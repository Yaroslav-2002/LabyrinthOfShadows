using Entities;
using UnityEngine;

namespace Entities.States
{
    public class PatrolState : ICreatureState
    {
        public CreatureStateType GetStateType() => CreatureStateType.Patrol;

        private Enemy enemy;

        public void Enter(Enemy enemy)
        {
            this.enemy = enemy;
        }

        public void Execute()
        {
            if (enemy.ShouldPatrol())
            {
                enemy.Patrol();
            }

            float distanceToPlayer = Vector3.Distance(enemy.transform.position, enemy.player.View.transform.position);
            if (distanceToPlayer <= enemy.detectionRange)
            {
                enemy.ChangeState<ChaseState>();
            }
        }

        public void Exit()
        {
        }
    }
}
