using UnityEngine;
using UnityEngine.InputSystem;

public class AnimationController : MonoBehaviour, IAnimationController
{
    [SerializeField] Transform weapon;
    [SerializeField] Animator _weaponAnimator;

    private Animator _animator;
    private Rigidbody2D _rigidBody;
    private SpriteRenderer _spriteRenderer;

    private Vector2 mousePointer;
    private PlayerInput _playerInput;
    private InputAction _mousePointerAction;
    
    private static readonly int SpeedAnimParam = Animator.StringToHash("Speed");
    private static readonly int AttackAnimParam = Animator.StringToHash("Attack");

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidBody = GetComponent<Rigidbody2D>();
        _playerInput = GetComponent<PlayerInput>();

        _mousePointerAction = _playerInput.actions["MousePointer"];

        _mousePointerAction.performed += MouseDragged;
        _mousePointerAction.canceled += MouseDragged;

    }

    private void OnEnable()
    {
        _mousePointerAction.Enable();
    }

    private void OnDisable()
    {
        _mousePointerAction.Disable();
    }

    private void MouseDragged(InputAction.CallbackContext context)
    {
        mousePointer = context.ReadValue<Vector2>();
    }

    private void Update()
    {
        float speed = _rigidBody.velocity.sqrMagnitude;

        _animator.SetFloat("Speed", speed);
        _animator.SetFloat(SpeedAnimParam, _rigidBody.velocity.sqrMagnitude);

        FlipCharacter(mousePointer.x < Screen.width / 2);
    }

    private void FlipCharacter(bool toFlipCharacter)
    {
        _spriteRenderer.flipX = toFlipCharacter;

        FlipWeapon(toFlipCharacter);
    }

    private void FlipWeapon(bool toFlipWeapon)
    {
        var weaponScreenPosition = Camera.main.WorldToScreenPoint(weapon.transform.position);
        weapon.right = (mousePointer - (Vector2)weaponScreenPosition).normalized;

        if (toFlipWeapon)
            weapon.localScale = new Vector3(1, -1, 1);
        else
            weapon.localScale = new Vector3(1, 1, 1);
    }

    public void Attack()
    {
        _weaponAnimator.SetTrigger(AttackAnimParam);
    }
}