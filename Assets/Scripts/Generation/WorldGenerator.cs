using System;
using System.Threading;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

namespace Generation
{
    public class WorldGenerator : MonoBehaviour
    {
        [SerializeField] private Tilemap tilemap;
        [SerializeField] private Tilemap collisionTilemap;
        [SerializeField] private Tile wallTile;
        [SerializeField] private Tile pathTile;
        [SerializeField] private Tile collisionWallTile; 
        [SerializeField] private int mazeSize;
        [SerializeField] private int cellSize;
        [SerializeField] private int mainRoomHeight;

        private bool[,] _walls;
        private IGenerationAlgorithm _algorithm;
        private int _mazeHeight;

        private PlayerController _player;

        private void Update()
        {
            if (_player == null)
            {
                _player = FindObjectOfType<PlayerController>();
            }
            if (_player.transform.position.y - _mazeHeight < 10)
            {
                Generate();
            }
        }

        private int _mazeActualSize;
        private void Start()
        {
            GenerateMainRoom();
        }

        public void Generate()
        {
            if (_algorithm == null)
            {
                _algorithm = new EllerMaze(mazeSize);
                _algorithm.Init();
            }

            GenerateRow();
        }
        
        private void GenerateRow()
        {
            _walls = new bool[2, mazeSize];
            _algorithm.Generate(ref _walls);

            UpdateMaze();
        }
        
        private void GenerateMainRoom()
        {
            for (int y = mainRoomHeight; y > 0; y--)
            {
                for (int x = -1; x < mazeSize * 2; x++)
                {
                    if (y == mainRoomHeight || x == -1 || x == mazeSize * 2 - 1)
                    {
                        GenerateCell(true, x * cellSize, y * cellSize);
                        continue;
                    }
                    GenerateCell(false, x * cellSize, y * cellSize);
                }
            }
        }

        private void GenerateCell(bool isWall, int xStartCoordinate, int yStartCoordinate)
        {
            for (int x = xStartCoordinate; x < xStartCoordinate + cellSize; x++)
            {
                for (int y = yStartCoordinate; y < yStartCoordinate + cellSize; y++)
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), isWall ? wallTile : pathTile); 
                    collisionTilemap.SetTile(new Vector3Int(x, y, 0), isWall ? collisionWallTile : null); 
                }
            }
        }
        
        private void UpdateMaze()
        {
            GenerateCell(true, -1 * cellSize, _mazeHeight * cellSize);
            GenerateCell(true, -1 * cellSize, (_mazeHeight - 1) * cellSize);
            
            for (int x = 0; x < mazeSize; x++)
            {
                // Set the right walls
                GenerateCell(false, x * 2 * cellSize, _mazeHeight * cellSize);
                GenerateCell(!_walls[0, x], (x * 2 + 1) * cellSize, _mazeHeight * cellSize);
                
                // Set the bottom walls
                GenerateCell(!_walls[1, x], x * 2 * cellSize, (_mazeHeight - 1) * cellSize);
                GenerateCell(true, (x * 2 + 1) * cellSize, (_mazeHeight - 1) * cellSize);
            }

            _mazeHeight -= 2;
            collisionTilemap.RefreshAllTiles();
        }
    }
}
