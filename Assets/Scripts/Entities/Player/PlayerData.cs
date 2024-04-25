using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[Serializable]
public class PlayerData
{
    public float health;
    public Vector3 position;
    public float damage;
    public int keys;
    public int roomId;

    public PlayerData(float health, Vector3 position, float damage, int keys)
    {
        this.health = health;
        this.position = position;
        this.damage = damage;
        this.keys = keys;
    }
}