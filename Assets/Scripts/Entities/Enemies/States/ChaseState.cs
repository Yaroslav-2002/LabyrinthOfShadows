using UnityEngine;

namespace Entities.States
{
    public class ChaseState : ICreatureState
    {
        public CreatureStateType GetStateType() => CreatureStateType.Chase;

        private Enemy enemy;

        public void Enter(Enemy enemy)
        {
            this.enemy = enemy;
        }

        public void Execute()
        {
            enemy.ChasePlayer();

            float distanceToPlayer = Vector3.Distance(enemy.transform.position, enemy.player.View.transform.position);
            if (distanceToPlayer <= enemy.attackRange)
            {
                enemy.ChangeState<AttackState>();
            }
            else if (distanceToPlayer > enemy.detectionRange)
            {
                enemy.ChangeState<PatrolState>();
            }
        }

        public void Exit()
        {
        }
    }

}
