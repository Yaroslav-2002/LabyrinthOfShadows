using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform Weapon;
    [SerializeField] private MovementController movementController;
    [SerializeField] private AnimationController animationController;

    public bool IsAttacking { get; private set; }
    public float delay = 0.3f;
    private bool attackBlocked;
    public float attackAreaRadius;
    public int damage = 25;

    private PlayerInput _playerInput;
    private InputAction _mousButtons;
    private Health health;

    public bool HasKey { get { return true; } }

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _mousButtons = _playerInput.actions["MouseButtons"];
        health = GetComponent<Health>();
        _mousButtons.performed += OnMouseButtonPressed;
    }

    private void Start()
    {
        health.InitializeHealth(1000);
    }

    private void OnMouseButtonPressed(InputAction.CallbackContext context)
    {
        Attack();
    }

    private void Attack()
    {
        if (attackBlocked)
            return;

        animationController.Attack();

        IsAttacking = true;
        attackBlocked = true;
        DetectColliders();
        StartCoroutine(DelayAttack());
    }

    private IEnumerator DelayAttack()
    {
        yield return new WaitForSeconds(delay);
        attackBlocked = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Vector3 position = Weapon == null ? Vector3.zero : Weapon.position;
        Gizmos.DrawWireSphere(position, attackAreaRadius);
    }

    public void DetectColliders()
    {
        foreach(Collider2D collider in Physics2D.OverlapCircleAll(Weapon.position, attackAreaRadius))
        {
            Health health;
            if (health = collider.GetComponent<Health>())
            {
                health.GetHit(damage, gameObject);
            }
        }
    }

    public PlayerData GetPlayerData()
    {
        return new PlayerData(
            health.currentHealth,
            transform.position,
            damage,
            HasKey
            );
    }
}