using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Scripts.Generation
{
    [CreateAssetMenu(fileName = "MapConfiguration", menuName = "Map Configurations/New Map Configuration", order = 1)]
    public class MapConfiguration : ScriptableObject
    {
        public RuleTile wallTile;
        public RuleTile pathTile;

        public Tile collisionWallTile;
        public Tile preferedPathTile;

        public int cellSize;

        public int roomHeight;
        public int roomWidth;

        public int corridorHeight;
        public int corridorWidth;

        public int mainRoomHeight;
        public int mainRoomWidth;

        public GameObject door;

    }
}
