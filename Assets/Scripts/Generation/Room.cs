using System.Collections.Generic;
using UnityEngine;

public class Room
{
    public int RoomId { get; set; }
    public List<Vector3Int> MobSpawnPositions { get; set; }

    public Room(int roomId)
    {
        RoomId = roomId;
        MobSpawnPositions = new List<Vector3Int>();
    }
}