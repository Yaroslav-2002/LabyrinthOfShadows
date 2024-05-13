using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Room
{
    public string roomId;
    private Vector2 position;
    private Tilemap _collisionTilemap;
    private Tilemap _environmentTilemap;
    private Tilemap _decorationTilemap;
    private DoorController _door;

    public List<EntityData> entities;
}
