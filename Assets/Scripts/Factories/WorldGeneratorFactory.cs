using Assets.Scripts.Factories;
using Entities.Player;
using Generation;
using Generation.Algorithms;
using UnityEngine.Tilemaps;

public class WorldGeneratorFactory : IWorldGeneratorFactory
{ 
    public IProceduralWorldGenerator CreateProceduralWorldGenBase(PlayerGo playerGO, IGenerationAlgorithm algorithm, Tilemap tileMap, Tilemap collisionTileMap, Tile wallTile, Tile pathTile, Tile collisionWallTile, int mazeSize, int cellSize, int mainRoomHeight)
    {
        throw new System.NotImplementedException();
    }

    public IStaticWorldGenerator CreateStatciWorldGenBase(IGenerationAlgorithm algorithm, Tilemap tileMap, Tilemap collisionTileMap, Tile wallTile, Tile pathTile, Tile collisionWallTile, int mazeSize, int cellSize, int mainRoomHeight)
    {
        throw new System.NotImplementedException();
    }
}