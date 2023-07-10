using UnityEngine;

public class PlayerAnimatorController
{
    private static readonly int MoveUp = Animator.StringToHash("MoveUp");
    private static readonly int MoveLeft = Animator.StringToHash("MoveLeft");
    private static readonly int MoveRight = Animator.StringToHash("MoveRight");
    private static readonly int MoveDown = Animator.StringToHash("MoveDown");

    private  PlayerController _playerController;
    private Animator _animator;
    
    public PlayerAnimatorController(PlayerController playerController)
    {
        _playerController = playerController;
        _animator = playerController.animator;
    }

    public void UpdateAnimation(Vector2 movementAxis)
    {
        _animator.SetBool(MoveUp, movementAxis is { x: 0, y: > 0 });
        _animator.SetBool(MoveDown, movementAxis is { x: 0, y: < 0 });
        _animator.SetBool(MoveRight, movementAxis is { x: > 0, y: 0 });
        _animator.SetBool(MoveLeft, movementAxis is { x: < 0, y: 0 });
    }
}
