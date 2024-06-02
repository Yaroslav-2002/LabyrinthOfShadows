using Entities;
using UnityEngine;

namespace Entities.States
{
    public class AttackState : ICreatureState
    {
        public CreatureStateType GetStateType() => CreatureStateType.Attack;

        private Enemy enemy;

        public void Enter(Enemy enemy)
        {
            this.enemy = enemy;
        }

        public void Execute()
        {
            enemy.Attack();

            float distanceToPlayer = Vector3.Distance(enemy.transform.position, enemy.player.View.transform.position);
            if (distanceToPlayer > enemy.attackRange)
            {
                enemy.ChangeState<ChaseState>();
            }
        }

        public void Exit()
        {
        }



    }
}