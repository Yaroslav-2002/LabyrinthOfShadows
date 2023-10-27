using System.Collections.Generic;
using Rooms;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Generation
{
    public class RoomGenerator : MonoBehaviour
    {
        public Tilemap tilemap; // Assign in inspector
        public Tile wallTile; // Assign your wall tile in inspector
        private Cell[,] _maze;
     
        private const int Rows = 100;
        private const int Cols = 100;

        private void Start()
        {
            _maze = new EllersMaze(Rows, Cols).Generate();
            GenerateRoom();
        }


        private void GenerateRoom()
        {
            for (var x = 0; x < Rows; x++)
            {
                for (var y = 0; y < Cols; y++)
                {
                    Debug.Log($"Row {x}, Column {y}, Down wall{_maze[x, y].DownWall}, right wall:{_maze[x, y].RightWall}");
                }
            }

            // Perimeter
            for (var x = -1; x <= Rows * 2; x++)
            {
                for (var y = -1; y <= Cols * 2; y++)
                {
                    if (y == Cols * 2  || x == -1 || y == -1 || x == Rows * 2)
                    {
                        tilemap.SetTile(new Vector3Int(x, y, 0), wallTile);
                    }
                }
            }

            for (var y = Cols * 2 - 2; y >= 0; y -= 2)
            {
                for (var x = 0; x < Rows * 2; x++)
                {
                    tilemap.SetTile(new Vector3Int(x, y), wallTile);
                }
            }

            for (var row = Rows * 2 - 1; row > 0; row -= 2)
            {
                for (var col = 0; col < Cols * 2; col += 2)
                {
                    if (col == Rows * 2 - 2)
                    {
                        continue;
                    }
                    if (_maze[Mathf.Abs(row - Cols * 2) / 2, col / 2].RightWall)
                    {
                        tilemap.SetTile(new Vector3Int(col + 1, row), wallTile);
                    }
            
                    if (!_maze[Mathf.Abs(row - Cols * 2) / 2, col / 2].DownWall)
                    {
                        tilemap.SetTile(new Vector3Int(col, row - 1), null);
                    }
                }
            }
        }
    }
    
}
