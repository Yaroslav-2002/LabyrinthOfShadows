using UnityEngine;

public class AnimationController : MonoBehaviour, IAnimationController
{
    private readonly Animator _animator;
    private readonly Rigidbody2D _rigidBody;
    private readonly SpriteRenderer _spriteRenderer;
    private readonly IInputController _inputController;

    private static readonly int SpeedAnimParam = Animator.StringToHash("Speed");

    public AnimationController(Animator animator, Rigidbody2D rigidBody, SpriteRenderer spriteRenderer, IInputController inputController)
    {
        _animator = animator;
        _rigidBody = rigidBody;
        _inputController = inputController;
        _spriteRenderer = spriteRenderer;
    }

    private void Update()
    {
        Flip(_inputController.GetHorizontal() < 0);
        _animator.SetFloat(SpeedAnimParam, _rigidBody.velocity.sqrMagnitude);

        float speed = _rigidBody.velocity.sqrMagnitude;
        _animator.SetFloat("Speed", speed);
    }

    private void Flip(bool flip)
    {
        _spriteRenderer.flipX = flip;
    }
}