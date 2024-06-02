using Entities.Player;
using Entities.States;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.AI;
using VContainer;

namespace Entities
{
    public abstract class Enemy : MonoBehaviour
    {
        [Inject] public PlayerGo player;
        public float detectionRange = 5f;
        public float attackRange = 1f;

        public EntityType type;
        private int level;

        [SerializeField] protected NavMeshAgent navMeshAgent;
        [SerializeField] protected Transform AttackAreaOrigin;
        [SerializeField] protected float speed = 2f;
        [SerializeField] protected float chaseSpeed = 3.5f;
        [SerializeField] protected float stopBeforeAttackDistance = 0.5f;
        [SerializeField] protected float attackCooldown = 1.5f;
        [SerializeField] protected int damage = 10;
        [SerializeField] protected float patrolRadius = 10f;
        [SerializeField] protected LayerMask navMeshLayerMask;
        [SerializeField] protected Health health;

        public static readonly int Horizontal = Animator.StringToHash("Horizontal");
        public static readonly int Vertical = Animator.StringToHash("Vertical");
        public static readonly int LastHorizontal = Animator.StringToHash("LastHorizontal");
        public static readonly int LastVertical = Animator.StringToHash("LastVertical");
        public static readonly int AttackAnimTrigger = Animator.StringToHash("Attack");
        public static readonly int HurtAnimTrigger = Animator.StringToHash("Hurt");
        public static readonly int DeathAnimTrigger = Animator.StringToHash("Die");

        protected Animator animator;
        protected ICreatureState currentState;

        public bool IsAttacking { get; private set; }
        protected float lastAttackTime = 0f;
        public float delay = 0.3f;
        private bool attackBlocked;
        public float attackAreaRadius;
        
        public void Initialize(CreatureData creatureData, int level = 1)
        {
            navMeshAgent.speed = speed;
            animator = GetComponent<Animator>();
            health = GetComponent<Health>();
            navMeshAgent.updateRotation = false;
            navMeshAgent.updateUpAxis = false;

            if (creatureData == null)
            {
                health.InitializeHealth(health.maxHealth);
                SetInitialState();
            }
            else
            {
                gameObject.transform.position =new Vector3(creatureData.x, creatureData.y, 0);
                SetStateFromType(creatureData.currentState);
                health.InitializeHealth(creatureData.health);
                level = creatureData.level;
            }

            health.OnDeathWithReference += Die;

            damage *= level;
        }

        protected abstract void SetInitialState();

        protected virtual void Update()
        {
            UpdateAnimationParams();
            currentState.Execute();
        }

        public void SetStateFromType(CreatureStateType stateType)
        {
            switch (stateType)
            {
                case CreatureStateType.Patrol:
                    ChangeState<PatrolState>();
                    break;
                case CreatureStateType.Chase:
                    ChangeState<ChaseState>();
                    break;
                case CreatureStateType.Attack:
                    ChangeState<AttackState>();
                    break;
            }
        }

        private void UpdateAnimationParams()
        {
            var velocityNormal = navMeshAgent.velocity.normalized;
            animator.SetFloat(Horizontal, velocityNormal.x);
            animator.SetFloat(Vertical, velocityNormal.y);

            if (navMeshAgent.velocity != Vector3.zero)
            {
                animator.SetFloat(LastHorizontal, velocityNormal.x);
                animator.SetFloat(LastVertical, velocityNormal.y);
            }
        }

        public void ChangeState<T>() where T : ICreatureState, new()
        {
            currentState?.Exit();
            currentState = EnemyStateFactory.GetState<T>();
            currentState.Enter(this);
        }

        public abstract void TakeDamage(int damage);

        public virtual void Die()
        {
            StartCoroutine(DieSequence());
            
        }

        private IEnumerator DieSequence()
        {
            if (navMeshAgent != null)
                navMeshAgent.enabled = false;

            IsAttacking = false;

            animator.SetTrigger(DeathAnimTrigger);

            yield return new WaitForSeconds(0.5f);

            Destroy(gameObject);
        }

        public bool ShouldPatrol()
        {
            return navMeshAgent.isOnNavMesh && !navMeshAgent.pathPending && navMeshAgent.remainingDistance < 0.5f;
        }

        public void Patrol()
        {
            navMeshAgent.speed = speed;
            GoToNextWaypoint();
        }

        private void GoToNextWaypoint()
        {
            Vector3 randomDirection = Random.insideUnitSphere * patrolRadius + transform.position;
            if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, patrolRadius, navMeshLayerMask))
            {
                navMeshAgent.SetDestination(hit.position);
            }
        }

        public void ChasePlayer()
        {
            navMeshAgent.speed = chaseSpeed;
            navMeshAgent.SetDestination(CalculateStopPosition());
        }

        private Vector3 CalculateStopPosition()
        {
            Vector3 directionToPlayer = (player.View.transform.position - transform.position).normalized;
            return player.View.transform.position - directionToPlayer * stopBeforeAttackDistance;
        }

        private void DetectColliders()
        {
            foreach (Collider2D collider in Physics2D.OverlapCircleAll(AttackAreaOrigin.position, attackAreaRadius))
            {
                Health health;
                if (health = collider.GetComponent<Health>())
                {
                    health.GetHit(damage, gameObject);
                }
            }
        }

        public void Attack()
        {
            if (!IsAttackCooldownComplete())
                return;

            lastAttackTime = Time.time;
            animator.SetTrigger(AttackAnimTrigger);
            IsAttacking = true;
            attackBlocked = true;
            DetectColliders();
        }

        private bool IsAttackCooldownComplete()
        {
            return Time.time >= lastAttackTime + attackCooldown;
        }

        public CreatureData GetCreatureData()
        {
            return new CreatureData(
                currentState.GetStateType(),
                health.currentHealth,
                transform.position,
                level
                ); ;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Vector3 position = AttackAreaOrigin == null ? Vector3.zero : AttackAreaOrigin.position;
            Gizmos.DrawWireSphere(position, attackAreaRadius);
        }
    }
}