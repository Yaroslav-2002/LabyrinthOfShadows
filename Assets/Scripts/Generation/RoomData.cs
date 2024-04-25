using System;
using UnityEngine;

[Serializable]
public class RoomData
{
    public Vector3 position;
    public int[] neighbors;
    public int[,] grid; 
    public EntityData[] enemies;
    public TrapData[] traps;
    public KeyData key;
    public bool hasKey;
}
