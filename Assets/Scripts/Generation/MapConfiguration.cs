using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Scripts.Generation
{
    [CreateAssetMenu(fileName = "MapConfiguration", menuName = "Map Configurations/New Map Configuration", order = 1)]
    public class MapConfiguration : ScriptableObject
    {
        public Tile wallTile;
        public Tile pathTile;
        public Tile collisionWallTile;
        public Tile preferedPathTile;
        public int cellSize;
        public int roomSize;
    }
}
