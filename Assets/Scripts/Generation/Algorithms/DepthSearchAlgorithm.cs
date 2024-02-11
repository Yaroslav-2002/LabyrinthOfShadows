using System.Collections.Generic;
using System.Linq;
using Helpers;
using Random = Unity.Mathematics.Random;

namespace Generation.Algorithms
{
    
    public class DepthSearchAlgorithm : IGenerationAlgorithm
    {
        private bool[,] _walls;
        private Graph<Cell> _path;
        
        private Cell[,] _cells;
        private readonly int _rows;
        private readonly int _cols;

        public DepthSearchAlgorithm(int rows, int cols)
        {
            this._rows = rows;
            this._cols = cols;
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
            _walls = new bool[_rows, _cols];
            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _cols; j++)
                {
                    _walls[i, j] = true;
                    _cells[i, j] = new Cell();
                    _path.AddVertex(_cells[i, j]);
                }
            }
            SetAdjacentCells();
        }
        
/*
    
    5) Break the wall between N and A.
    6) Assign the value A to N.
    7) Go to step 2.
*/
        
        private List<(Cell, Cell)> _cellPath;
        private static Random _random;
        public void Generate(ref bool[,] maze)
        {
            Queue<Cell> queue = new Queue<Cell>();
            
            //1) Randomly select a node (or cell) N.
            _random = new Random(0x6E624EB7u);
            int randomRow = _random.NextInt(_rows);
            int randomCol = _random.NextInt(_cols);

            //3) Mark the cell N as visited.
            var N = _cells[randomRow, randomCol];
                N.Visited = true; 
                
            //2) Push the node N onto a queue Q.
            queue.Enqueue(N);

            
            // 4) Randomly select an adjacent cell A of node N that has not been visited. If all the neighbors of N have been visited:
            // Continue to pop items off the queue Q until a node is encountered with at least one non-visited neighbor - assign this node to N and go to step 4.
            // If no nodes exist: stop.
            while (queue.Count > 0)
            {
                Cell currentCell = queue.Dequeue();
                var unvisitedNeighbors = GetUnvisitedNeighbors(currentCell);

                if (unvisitedNeighbors.Count > 0)
                {
                    var rand =_random.NextInt(unvisitedNeighbors.Count);
                    Cell nextCell = unvisitedNeighbors[rand];
                    _path.AddEdge(currentCell, nextCell);
                    nextCell.Visited = true;
                    queue.Enqueue(nextCell);
                }
            }

            CellPathToBoolArray();
            maze = _walls;
        }

        private void CellPathToBoolArray()
        {
            for (int i = 0; i < _cells.GetLength(0) - 1; i++)
            {
                for (int j = 0; j < _cells.GetLength(1) - 1; j++)
                {
                    if (_path.HasEdge(_cells[i, j], _cells[i, j + 1]))
                        _walls[i, j] = true;
                    else
                    {
                        _walls[i, j] = false;
                    }
                    
                    if (_path.HasEdge(_cells[i, j], _cells[i + 1, j]))
                        _walls[i + 1, j] = true;
                    else
                    {
                        _walls[i + 1, j] = false;
                    }
                }
            }
        }

    private List<Cell> GetUnvisitedNeighbors(Cell cell)
    {
            return cell.AdjacentCells.Where(x => !x.Visited).ToList();
    }

        public class Cell
        {
            public bool Visited { get; set; }
            public List<Cell> AdjacentCells { get; private set; }

            public Cell()
            {
                Visited = false;
                AdjacentCells = new List<Cell>();
            }
        }
    }
}