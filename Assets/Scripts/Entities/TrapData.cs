using System;
using System.Numerics;

[Serializable]
public struct TrapData
{
    public bool isActivated;
    public TrapType trapType;
    public Vector3 position;
    public int roomId; 
}

public enum TrapType
{

}