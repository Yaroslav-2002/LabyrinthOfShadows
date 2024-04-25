using System;
using UnityEngine;

[Serializable]
public struct EntityData
{
    public float health;
    public Vector3 position;
    public float damage;
    public string currentState;
    public int roomId;
}
