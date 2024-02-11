using Generation.Algorithms;
using UnityEngine;
using UnityEngine.Tilemaps;
using VContainer.Unity;

namespace Generation
{
    public class StaticWorldGenerator : WorldGeneratorBase
    {
        public StaticWorldGenerator(IGenerationAlgorithm algorithm, Tilemap tileMap, Tilemap collisionTileMap, Tile wallTile, Tile pathTile, Tile collisionWallTile, int mazeSize, int cellSize, int mainRoomHeight)
            : base(algorithm, tileMap, collisionTileMap, wallTile, pathTile, collisionWallTile, mazeSize, cellSize, mainRoomHeight)
        {
            
        }

        public override void Generate()
        {
            base.Generate();
            
            walls = new bool[mazeSize, mazeSize];
            algorithm.Generate(ref walls);

            SetWalls();
        }
        
        private void SetWalls()
        {
            GenerateCell(true, -1 * cellSize, mazeHeight * cellSize);
            GenerateCell(true, -1 * cellSize, (mazeHeight - 1) * cellSize);
            
            for (int x = 0; x < mazeSize; x++)
            {
                for (int y = 1; y < mazeSize; y++)
                {
                    // Set the right walls
                    GenerateCell(false, x * 2 * cellSize, mazeHeight * cellSize);
                    GenerateCell(walls[y - 1, x], (x * 2 + 1) * cellSize, mazeHeight * cellSize);
                
                    // Set the bottom walls
                    GenerateCell(walls[y, x], x * 2 * cellSize, (mazeHeight - 1) * cellSize);
                    GenerateCell(true, (x * 2 + 1) * cellSize, (mazeHeight - 1) * cellSize);
                }
            }

            collisionTileMap.RefreshAllTiles();
        }
    }
}
