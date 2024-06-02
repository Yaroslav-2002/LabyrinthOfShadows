using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[Serializable]
public class PlayerData
{
    public float health;
    public float x;
    public float y;
    public int damage;
    public bool hasKey;

    public PlayerData(float health, Vector3 position, int damage, bool hasKey)
    {
        this.health = health;
        this.x = position.x;
        this.y = position.y;
        this.damage = damage;
        this.hasKey = hasKey;
    }
}