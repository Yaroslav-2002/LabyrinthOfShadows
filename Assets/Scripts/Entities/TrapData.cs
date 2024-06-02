using System;
using UnityEngine;

[Serializable]
public class TrapData
{
    public float x;
    public float y;

    public TrapData(Vector3 position)
    {
        this.x = position.x;
        this.y = position.y;
    }
}

public enum TrapType
{

}