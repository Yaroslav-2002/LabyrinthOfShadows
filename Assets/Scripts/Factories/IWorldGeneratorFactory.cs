using Entities.Player;
using Generation;
using Generation.Algorithms;
using UnityEngine.Tilemaps;

namespace Assets.Scripts.Factories
{
    public interface IWorldGeneratorFactory
    {
        IProceduralWorldGenerator CreateProceduralWorldGenBase(PlayerGo playerGO, IGenerationAlgorithm algorithm,
                     Tilemap tileMap, Tilemap collisionTileMap, Tile wallTile, Tile pathTile,
                     Tile collisionWallTile, int mazeSize, int cellSize, int mainRoomHeight);

        IStaticWorldGenerator CreateStatciWorldGenBase(IGenerationAlgorithm algorithm,
                     Tilemap tileMap, Tilemap collisionTileMap, Tile wallTile, Tile pathTile,
                     Tile collisionWallTile, int mazeSize, int cellSize, int mainRoomHeight);
    }
}
