using System;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    public Vector2 movingAxis;

    public Animator animator;
    private PlayerAnimatorController _animatorController;
    private MovementController _movementController;
    private InputManager _inputManager;
    
    private void Start()
    {
        animator = GetComponent<Animator>();
        _inputManager = GetComponent<InputManager>();
        _movementController = new MovementController(this);
        _animatorController = new PlayerAnimatorController(this);
        rb = GetComponent<Rigidbody2D>();
    }
    // Assign the dependencies through a method
    public void Initialize(MovementController movementController, PlayerAnimatorController animatorController)
    {
        _movementController = movementController;
        _animatorController = animatorController;
    }
    

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            movingAxis = _inputManager.GetCurrentTouchPosition(0);
        }
        
        _movementController.UpdateMovement();
        _animatorController.UpdateAnimation(movingAxis);
    }
}