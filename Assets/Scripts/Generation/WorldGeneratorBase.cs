using Generation.Algorithms;
using UnityEngine;
using UnityEngine.Tilemaps;
using VContainer.Unity;

namespace Generation
{
    public abstract class WorldGeneratorBase
    {
        protected readonly IGenerationAlgorithm algorithm;
        protected readonly Tilemap tileMap;
        protected readonly Tilemap collisionTileMap;
        protected readonly Tile wallTile;
        protected readonly Tile pathTile;
        protected readonly Tile collisionWallTile; 
        protected readonly int mazeSize;
        protected readonly int cellSize;
        protected readonly int mainRoomHeight;
        
        protected bool[,,] walls;
        protected int mazeHeight;

        public WorldGeneratorBase(IGenerationAlgorithm algorithm, Tilemap tileMap, Tilemap collisionTileMap, Tile wallTile, Tile pathTile, Tile collisionWallTile, int mazeSize, int cellSize, int mainRoomHeight)
        {
            this.algorithm = algorithm;
            this.tileMap = tileMap; 
            this.collisionTileMap = collisionTileMap;
            this.wallTile = wallTile;
            this.pathTile = pathTile;
            this.collisionWallTile = collisionWallTile;
            this.mazeSize = mazeSize;
            this.cellSize = cellSize;
            this.mainRoomHeight = mainRoomHeight;
        }

        public virtual void Generate()
        {
            GenerateMainRoom();
            algorithm?.Init();
        }
        
        protected void GenerateCell(bool isWall, int xStartCoordinate, int yStartCoordinate)
        {
            for (int x = xStartCoordinate; x < xStartCoordinate + cellSize; x++)
            {
                for (int y = yStartCoordinate; y < yStartCoordinate + cellSize; y++)
                {
                    tileMap.SetTile(new Vector3Int(x, y, 0), isWall ? wallTile : pathTile); 
                    collisionTileMap.SetTile(new Vector3Int(x, y, 0), isWall ? collisionWallTile : null); 
                }
            }
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
    }
}