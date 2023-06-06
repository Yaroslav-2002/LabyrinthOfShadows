using System.Collections.Generic;
using Rooms;
using UnityEngine;

namespace Generation
{
    public class RoomGenerator : MonoBehaviour
    {
        [System.Serializable]
        public struct RoomConfig
        {
            public RoomType roomType;
            public GameObject prefab;
            public List<Vector2Int> doorPositions;
        }

        public int gridSizeX = 10;
        public int gridSizeY = 10;
        public int maxRooms = 15;
        public int minRooms = 5;

        public List<RoomConfig> roomConfigs;
        private List<Room> placedRooms;

        private void Start()
        {
            GenerateRooms();
        }

        private bool IsValidRoomPlacement(Room room, Vector2Int position)
        {
            // Check if the room fits within the gridSize and doesn't overlap with other placedRooms
            // Return true if it fits, otherwise return false
            return false;
        }

        private void GenerateRooms()
        {
            placedRooms = new List<Room>();

            // Choose a random starting point on the grid
            Vector2Int startPoint = new Vector2Int(Random.Range(0, gridSizeX), Random.Range(0, gridSizeY));

            // Choose a random room config and assign it to startingRoom
            RoomConfig startingRoomConfig = roomConfigs[Random.Range(0, roomConfigs.Count)];

            // Create startingRoom and add it to placedRooms
            Room startingRoom = RoomFactory.CreateRoom<StartRoom>();
            placedRooms.Add(startingRoom);

            while (placedRooms.Count < maxRooms)
            {
                // Choose a random roomConfig
                RoomConfig roomConfig = roomConfigs[Random.Range(0, roomConfigs.Count)];

                // Choose a random position on the grid
                Vector2Int position = new Vector2Int(Random.Range(0, gridSizeX), Random.Range(0, gridSizeY));

                // Create a new room
                Room newRoom = RoomFactory.CreateRoom<StartRoom>();

                if (IsValidRoomPlacement(newRoom, position))
                {
                    // Place the room on the grid
                    placedRooms.Add(newRoom);
                }
            }
        }
    }
}
