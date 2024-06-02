using Entities;
using Entities.States;
using System;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.UIElements;

[Serializable]
public class CreatureData
{
    public float health;
    public float x;
    public float y;
    public int level;
    public CreatureStateType currentState;

    public CreatureData(CreatureStateType _currentState, float _health, Vector3 position, int _level)
    {
        health = _health;
        this.x = position.x;
        this.y = position.y;
        level = _level;
        currentState = _currentState;
    }
}
