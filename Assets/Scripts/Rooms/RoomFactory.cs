using System.Collections.Generic;
using UnityEngine;

namespace Rooms
{
    public static class RoomFactory
    {
        private static Dictionary<RoomType, GameObject> roomPrefabs;

        public static Room CreateRoom<T>() where T : Room, new()
        {
            // if (roomPrefabs.ContainsKey(roomType))
            // {
            //     GameObject prefab = roomPrefabs[roomType];
            //     T room = new T();
            //     room.Init(width, height, position, roomType, doorPositions, roomPrefab);
            //     return room;
            // }
            // else
            // {
            //     Debug.LogError("Room prefab for room type " + roomType + " not found.");
            //     return null;
            // }
            return new StartRoom();
        }
    }
}
