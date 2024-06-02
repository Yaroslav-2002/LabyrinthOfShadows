using Entities.States;

namespace Entities
{
    public class Skeleton : Enemy
    {
        protected override void SetInitialState()
        {
            ChangeState<PatrolState>();
        }

        public override void TakeDamage(int damage)
        {
            animator.SetTrigger(HurtAnimTrigger);
        }

        public override void Die()
        {
            base.Die();
        }
    }
}