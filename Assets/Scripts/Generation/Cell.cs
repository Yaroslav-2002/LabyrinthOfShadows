using System.Collections.Generic;

namespace Generation
{
    public class Cell
    {
        public bool Visited { get; set; }
        public List<Cell> AdjacentCells { get; }
        public int Row { get; }
        public int Col { get; }

        public bool HasRightWall { get; set; }
        public bool HasBottomWall { get; set; }
        public Cell(int row, int col)
        {
            Row = row;
            Col = col;
            Visited = false;
            AdjacentCells = new List<Cell>();
        }
    }
}