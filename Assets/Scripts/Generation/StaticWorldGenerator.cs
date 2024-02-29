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
            
            walls = new bool[mazeSize, mazeSize, 2];

            for (int y = 0; y < mazeSize; y++)
            {
                for (int x = 0; x < mazeSize; x++)
                {
                    walls[x, y, 0] = true;
                    walls[x, y, 1] = true;
                }
            }

            algorithm.Generate(ref walls);
            SetWalls();
        }
        
        private void SetWalls()
        {      
            for (int y = 0; y < mazeSize; y++)
            {
                for (int x = 0; x < mazeSize; x++)
                {
                    GenerateCell(true, -1 * cellSize, -(y * 2 * cellSize));
                    GenerateCell(true, -1 * cellSize, -((y * 2 + 1) * cellSize));

                    // Set the right walls
                    GenerateCell(false, x * 2 * cellSize, -(y * 2 * cellSize));
                    GenerateCell(walls[x, y, 1], x * 2 * cellSize, -((y * 2 + 1) * cellSize));

                    // Set the bottom walls
                    GenerateCell(walls[x, y, 0], ((x * 2) + 1) * cellSize, -(y * 2 * cellSize));
                    GenerateCell(true, (x * 2 + 1) * cellSize, -((y * 2 + 1) * cellSize));
                }
            }

            collisionTileMap.RefreshAllTiles();
        }
    }
}
