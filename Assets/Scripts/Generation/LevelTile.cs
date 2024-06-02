using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New level tile", menuName = "2D/Tiles/Level Tile")]
public class LevelTile : Tile
{
    public TileType Type;
}
public enum TileType
{
    None = -1,
    Path = 0,
    Wall = 0,
}
