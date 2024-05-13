using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform Weapon;
    [SerializeField] private MovementController movementController;
    [SerializeField] private AnimationController animationController;

    private PlayerInput _playerInput;
    private InputAction _mousButtons;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _mousButtons = _playerInput.actions["MouseButtons"];

        _mousButtons.performed += OnMouseButtonPressed;
    }

    private void OnMouseButtonPressed(InputAction.CallbackContext context)
    {
        Attack();
    }

    private void Attack()
    {
        animationController.Attack();
    } 
}