using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

namespace Generation.Algorithms
{
    public class EllerMazeAlgorithm : IGenerationAlgorithm
    {
        private int _nodesNumWidth;
        private Dictionary<int, HashSet<int>> _sets;
        private int[] _maze;

        public EllerMazeAlgorithm(int nodesNumWidth)
        {
            _nodesNumWidth = nodesNumWidth;

            _maze = new int[_nodesNumWidth + 2]; // 2 excess array members to start loop from 1
            _sets = new Dictionary<int, HashSet<int>>();
            for (int col = 1; col <= _nodesNumWidth; col++)
            {
                _maze[col] = col;
                _sets.Add(col, new HashSet<int> { col });
            }
        }

        public IEnumerator<bool[,]> GenerateRow()
        {
            while (true)
            {
                var walls = new bool[_nodesNumWidth, 2];

                for (int i = 0; i < _nodesNumWidth; i++)
                {
                    walls[i, 0] = true;
                    walls[i, 1] = true;
                }

                IEnumerator<bool> horizontalConnectionGenerator = MakeHorizontalConnection();

                for (int x = 0; x < _nodesNumWidth - 1; x++)
                {
                    if (horizontalConnectionGenerator.MoveNext())
                    {
                        var isWall = horizontalConnectionGenerator.Current;
                        walls[x, 0] = isWall;
                    }
                }

                IEnumerator<int> verticalConnectionGenerator = MakeVerticalConnection();

                while (verticalConnectionGenerator.MoveNext())
                {
                    var pos = verticalConnectionGenerator.Current;
                    walls[pos, 1] = false;
                }
                yield return walls;
            }
        }

        private IEnumerator<bool> MakeHorizontalConnection()
        {
            for (int col = 1; col < _nodesNumWidth; col++)
            {
                if (_maze[col] != _maze[col + 1] && _sets[_maze[col]] != _sets[_maze[col + 1]] && Random.Range(0, 2) == 0)
                {
                    JoinSets(_maze[col], _maze[col + 1]);
                    _maze[col + 1] = _maze[col];
                    yield return false;
                }
                else
                {
                    yield return true;
                }
            }
        }

        private IEnumerator<int> MakeVerticalConnection()
        {
            var maze2 = new int[_nodesNumWidth + 2];

            Dictionary<int, HashSet<int>> sets2 = new Dictionary<int, HashSet<int>>();
            foreach (var set in _sets)
            {
                List<int> list = new List<int>(set.Value);
                var connections = Random.Range(1, list.Count + 1);

                for (int i = 0; i < connections; i++)
                {
                    var randIndex = Random.Range(0, list.Count);
                    int actualCellPosition = list[randIndex];
                    maze2[actualCellPosition] = _maze[actualCellPosition];
                    yield return actualCellPosition - 1;

                    if (sets2.ContainsKey(maze2[actualCellPosition]))
                        sets2[maze2[actualCellPosition]].Add(actualCellPosition);
                    else
                    {
                        sets2.Add(maze2[actualCellPosition], new HashSet<int>() { actualCellPosition });
                    }
                }
            }

            _sets = sets2;
            _maze = maze2;

            for (int i = 1; i <= _nodesNumWidth; i++)
            {
                if (_maze[i] == 0)
                {
                    _maze[i] = _sets.Keys.Max() + 1;
                    _sets.Add(_maze[i], new HashSet<int> { i });
                }
            }
        }

        private void JoinSets(int setId1, int setId2)
        {
            foreach (var col in _sets[setId2])
            {
                _sets[setId1].Add(col);
                _maze[col] = setId1;
            }

            _sets.Remove(setId2);
        }
    }
}
