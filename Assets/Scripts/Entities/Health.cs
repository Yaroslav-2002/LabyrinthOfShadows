using UnityEngine.Events;
using UnityEngine;
using System;

public class Health : MonoBehaviour
{
    [SerializeField]
    public float currentHealth, maxHealth;

    public Action<float> OnHitWithReference;
    public Action OnDeathWithReference;

    [SerializeField]
    private bool isDead = false;

    public void InitializeHealth(float healthValue)
    {
        currentHealth = healthValue;
        maxHealth = healthValue;
        isDead = false;
    }

    public void GetHit(int amount, GameObject sender)
    {
        if (isDead)
            return;
        if (sender.layer == gameObject.layer)
            return;

        currentHealth -= amount;

        if (currentHealth > 0)
        {
            OnHitWithReference?.Invoke(currentHealth / maxHealth);
        }
        else
        {
            OnDeathWithReference?.Invoke();
            isDead = true;
        }
    }
}