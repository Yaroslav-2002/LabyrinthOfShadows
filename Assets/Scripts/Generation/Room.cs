using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using VContainer;

public class Room
{
    private readonly Grid grid;

    public static int roomId;
    public Tilemap CollisionTilemap { get; set; }
    public Tilemap MazeTilemap { get; set; }
    public Tilemap DecorationTilemap { get; set; }
    public DoorController Door { get; set; }

    public List<EntityData> entities;

    public Room(Grid _grid)
    {
        grid = _grid;
    }

    public void InstantiateRoom(Vector3 position)
    {
        // Create the Room GameObject
        GameObject roomObject = new GameObject($"Room{roomId}");

        // Create and set up the Maze Tilemap
        GameObject mazeTilemapObject = new GameObject("MazeTilemap");
        mazeTilemapObject.transform.SetParent(roomObject.transform);
        MazeTilemap = mazeTilemapObject.AddComponent<Tilemap>();
        mazeTilemapObject.AddComponent<TilemapRenderer>();

        // Create and set up the Collision Tilemap
        GameObject collisionTilemapObject = new GameObject("CollisionTilemap");
        collisionTilemapObject.transform.SetParent(roomObject.transform);
        CollisionTilemap = collisionTilemapObject.AddComponent<Tilemap>();
        collisionTilemapObject.AddComponent<TilemapRenderer>();

        // Set the position of the room
        roomObject.transform.SetParent(grid.transform);
        roomObject.transform.position = position;
        roomId++;
    }
}
