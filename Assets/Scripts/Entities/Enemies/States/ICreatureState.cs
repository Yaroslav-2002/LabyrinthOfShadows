using System;

namespace Entities.States
{
    public interface ICreatureState
    {
        void Enter(Enemy enemy);
        void Execute();
        CreatureStateType GetStateType();
        void Exit();
    }
}