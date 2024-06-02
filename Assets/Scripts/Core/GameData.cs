using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class GameData
{
    public TilemapData collisionTilemapData;
    public TilemapData wallTilemapData;
    public TilemapData pathTilemapData;
    //public List<KeyData> keyDatas;
    public List<DoorData> doorDatas;
    public List<TrapData> trapDatas;
    public List<CreatureData> enemyDatas;
    public PlayerData playerData;
}
