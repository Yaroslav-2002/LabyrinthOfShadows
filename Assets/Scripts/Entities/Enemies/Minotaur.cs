using Entities.States;
using System.Collections;
using UnityEngine;

namespace Entities
{
    public class Minotaur : Enemy
    {
        [SerializeField] private Color hitColor = Color.white;
        [SerializeField] private float hitColorDuration = 0.1f;

        private SpriteRenderer spriteRenderer;
        private Color originalColor;

        protected void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            originalColor = spriteRenderer.color;
        }

        protected override void SetInitialState()
        {
            ChangeState<PatrolState>();
        }

        public override void TakeDamage(int damage)
        {
            StartCoroutine(ShowHitEffect());
        }

        private IEnumerator ShowHitEffect()
        {
            spriteRenderer.color = hitColor;
            yield return new WaitForSeconds(hitColorDuration);
            spriteRenderer.color = originalColor;
        }

        public override void Die()
        {
            // Реалізація логіки смерті
        }
    }

}