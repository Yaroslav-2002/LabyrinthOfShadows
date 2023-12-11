using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{ 
    public class EllersMaze : MonoBehaviour 
    {
        
    private int _cols;
    private Dictionary<int, HashSet<int>> _sets;
    private int[] _maze;
    private bool[] _walls;

    public EllersMaze(int size)
    {
        _cols = size;
    }

    public void Init()
    {
        _cols = 5;
        _maze = new int[_cols + 2];
        _sets = new Dictionary<int, HashSet<int>>();
        for (int col = 1; col <= _cols; col++)
        {
            _maze[col] = col;
            _sets.Add(col, new HashSet<int> {col});
        }
    }
    
    public void GenerateRow()
    {
        MakeHorizontalConnection();
        
        MakeVerticalConnection();
    }

    private void MakeHorizontalConnection()
    {
        for (int col = 1; col < _cols; col++)
        {
            if (_maze[col] != _maze[col + 1] && _sets[_maze[col]] != _sets[_maze[col + 1]] && Random.Range(0, 2) == 0)
            {
                JoinSets(_maze[col], _maze[col + 1]);
                _maze[col + 1] = _maze[col];
            }
        }

        foreach (var cell in _maze)
        {
            Debug.Log($"{cell}");
        }
    }
    
    private void MakeVerticalConnection()
    {
        
        var maze2 = new int[_cols + 2];
        var startIndex = 1;
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

                if (sets2.ContainsKey(maze2[actualCellPosition]))
                    sets2[maze2[actualCellPosition]].Add(actualCellPosition);
                else
                {
                    sets2.Add(maze2[actualCellPosition], new HashSet<int>(){actualCellPosition});
                }
            }
        }

        _sets = sets2;
        _maze = maze2;

        for (int i = 1; i <= _cols; i++)
        {
            if (_maze[i] == 0)
            {
                _maze[i] = _sets.Keys.Max() + 1;
                _sets.Add(_maze[i], new HashSet<int> {i});
            }
        }
        
        foreach (var cell in _maze)
        {
            Debug.Log($"{cell}");
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
