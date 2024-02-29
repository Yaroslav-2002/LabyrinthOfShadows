using System.Collections.Generic;
using System.Linq;
using Helpers;

namespace Generation.Algorithms
{

    public class DepthSearchAlgorithm : IGenerationAlgorithm
    {
        private bool[,,] _walls;
        private Graph<Cell> _path;
        
        private Cell[,] _cells;
        private readonly int _rows;
        private readonly int _cols;

        public DepthSearchAlgorithm(int rows, int cols)
        {
            _rows = rows;
            _cols = cols;
        }

        private void SetAdjacentCells()
        {
            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _cols; j++)
                {
                    if (i > 0) _cells[i, j].AdjacentCells.Add(_cells[i - 1, j]); // Add north
                    if (j > 0) _cells[i, j].AdjacentCells.Add(_cells[i, j - 1]); // Add west
                    if (i < _rows - 1) _cells[i, j].AdjacentCells.Add(_cells[i + 1, j]); // Add south
                    if (j < _cols - 1) _cells[i, j].AdjacentCells.Add(_cells[i, j + 1]); // Add east
                    // You can add diagonals or other directions based on your requirements
                }
            }
        }

        public void Init()
        {
            _path = new Graph<Cell>();
            _cells = new Cell[_rows, _cols];
            _walls = new bool[_rows, _cols, 2];
            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _cols; j++)
                {
                    _walls[i, j, 0] = true;
                    _walls[i, j, 1] = true;
                    _cells[i, j] = new Cell((i, j));
                    _path.AddVertex(_cells[i, j]);
                }
            }
            SetAdjacentCells();
        }

        public void Generate(ref bool[,,] maze)
        {
            int visitedCells = 0;
            int totalCells = _rows * _cols;

            Stack<Cell> stack = new Stack<Cell>();

            //1) Randomly select a node (or cell) N.
            var rand = new System.Random();
            int randomCol = rand.Next(_cols);

            UnityEngine.Debug.Log($"Random cell: {randomCol}");

            //3) Mark the cell N as visited.
            var currentCell = _cells[0, randomCol];
                currentCell.Visited = true;

            visitedCells++;

            while (visitedCells < totalCells)
            {
                var unvisitedNeighbors = GetUnvisitedNeighbors(currentCell);
                UnityEngine.Debug.Log($"Unvisited neighbors count for cell {currentCell.value}: {unvisitedNeighbors.Count}");

                if (unvisitedNeighbors.Count > 0)
                {
                    var randomNum = rand.Next(unvisitedNeighbors.Count);
                    Cell nextCell = unvisitedNeighbors[randomNum];
                    nextCell.Visited = true;
                    _path.AddEdge(currentCell, nextCell);
                    stack.Push(currentCell);
                    currentCell = nextCell;
                    visitedCells++;
                }
                else
                {
                    stack.TryPop(out currentCell);
                }
            }

            CellPathToBoolArray();
            maze = _walls;
        }

        private void CellPathToBoolArray()
        {
            // Initially, all entries in _walls are set to true by Init()

            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _cols; j++)
                {
                    // For each cell, check the right and bottom neighbor
                    if (i < _rows - 1 && _path.HasEdge(_cells[i, j], _cells[i + 1, j]))
                        _walls[i, j, 0] = false; // Right passage
                    if (j < _cols - 1 && _path.HasEdge(_cells[i, j], _cells[i, j + 1]))
                        _walls[i, j, 1] = false; // Bottom passage
                }
            }
        }

        private List<Cell> GetUnvisitedNeighbors(Cell cell)
        {
            return cell.AdjacentCells.Where(x => !x.Visited).ToList();
        }
       
    }
    public class Cell
    {
        public bool Visited { get; set; }
        public List<Cell> AdjacentCells { get; private set; }

        public (int i, int j) value;

        public Cell((int i, int j) value)
        {
            AdjacentCells = new();
            this.value = value;
        }
    }
}