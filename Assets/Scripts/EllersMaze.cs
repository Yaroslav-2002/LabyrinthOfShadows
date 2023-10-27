using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using Random = UnityEngine.Random;

public class EllersMaze
{
    private Cell[,] _cells; // first bool true - right wall exist, second bool - down cell exist
    private int _rows;
    private int _cols;
    private int[,] _maze;
    private Dictionary<int, HashSet<int>> _sets;
    
    public EllersMaze(int rows, int cols)
    {
        _rows = rows;
        _cols = cols;
        _maze = new int[rows, cols];
        _cells = new Cell[rows, cols];
        _sets = new Dictionary<int, HashSet<int>>();
    }
    
    public Cell[,] Generate()
    {
        int currentRow = 0;
        
        for (int y = 0; y < _cells.GetLength(0); y++)
        {
            for (int x = 0; x < _cells.GetLength(1); x++)
            {
                _cells[x, y] = new Cell(true, true);
            }
        }
        
        // Step 1: Initialize first row with unique sets
        for (int col = 0; col < _cols; col++)
        {
            _maze[currentRow, col] = col + 1;
            _sets[col + 1] = new HashSet<int> { col };
        }

        while (currentRow < _rows - 1)
        {
            // Step 2: Randomly join adjacent cells, but they must not be in the same set
            for (int col = 0; col < _cols - 1; col++)
            {
                bool shouldJoin = Random.Range(0, 2) == 0;

                if (shouldJoin && _maze[currentRow, col] != _maze[currentRow, col + 1])
                {
                    JoinSets(_maze[currentRow, col], _maze[currentRow, col + 1]);
                    _cells[currentRow, col].RightWall = false;
                    _maze[currentRow, col + 1] = _maze[currentRow, col];
                }
            }

            // Step 3: For each set, randomly create vertical connections for the next row
            foreach (var set in _sets.Values.Distinct())
            {
                var columnsInSet = set.ToArray();
                int connections = Random.Range(1, columnsInSet.Length + 1);

                for (int i = 0; i < connections; i++)
                {
                    int randCol = columnsInSet[Random.Range(0, columnsInSet.Length)];
                    _maze[currentRow + 1, randCol] = _maze[currentRow, randCol];
                    _cells[currentRow, randCol].DownWall = false;
                    //_sets[_maze[currentRow, randCol]].Add(randCol);
                }
            }

            currentRow++;

            // Step 4: For cells in the new row that have no set, give them a new set
            for (int col = 0; col < _cols; col++)
            {
                if (_maze[currentRow, col] == 0)
                {
                    int newSetId = _sets.Keys.Max() + 1;
                    _maze[currentRow, col] = newSetId;
                    _sets[newSetId] = new HashSet<int> { col };
                }
            }
        }

        return _cells;
    }

    private void JoinSets(int setId1, int setId2)
    {
        if(_sets.Keys.Contains(setId1) && _sets.Keys.Contains(setId2))
            foreach (var col in _sets[setId2])
            {
                _sets[setId1].Add(col);
            }

        _sets.Remove(setId2);
    }
}
public struct Cell
{
    public bool RightWall { get; set; }
    public bool DownWall { get; set; }
    public int X { get; }
    public int Y { get; }

    public Cell(bool rightWall, bool downWall)
    {
        RightWall = rightWall;
        DownWall = downWall;
        X = 0;
        Y = 0;
    }
}