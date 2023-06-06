using System.Collections.Generic;
using UnityEngine;

namespace Rooms
{
    public abstract class Room
    {
        public int width;
        public int height;
        public Vector2Int position;
        public RoomType roomType;

        protected List<Vector2Int> doorPositions;
        protected GameObject roomPrefab;

        public void Init(int width, int height, Vector2Int position, RoomType roomType, List<Vector2Int> doorPositions, GameObject roomPrefab)
        {
            this.width = width;
            this.height = height;
            this.position = position;
            this.roomType = roomType;
            this.doorPositions = doorPositions;
            this.roomPrefab = roomPrefab;
        }

        // Common methods for all room types
    }

    public enum RoomType
    {
        Normal,
        Start,
        End,
        Treasure,
        Enemy
    }
}
