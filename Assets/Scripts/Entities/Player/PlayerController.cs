using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Controls;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;
using VContainer;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed;
    
    private InputController _inputController;

    private Animator _animator;
    private Rigidbody2D _rigidbody2D;
    private static readonly int HorizMoveAnimParam = Animator.StringToHash("HorizontalMove");
    private static readonly int VertMoveAnimParam = Animator.StringToHash("VerticalMove");
    private static readonly int SpeedAnimParam = Animator.StringToHash("Speed");
    
    private float _verticalMove;
    private float _horizontalMove;

    private void Awake()
    {
        _inputController = 
#if UNITY_EDITOR
            new KeyBoardInputController();
#else 
        new JoystickInputController(dynamicJoystick);
#endif
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        _horizontalMove = _inputController.GetHorizontal();
        _verticalMove = _inputController.GetVertical();
        
        SetMovement();
        SetAnimation();
    }

    private void SetAnimation()
    {
        _animator.SetFloat(HorizMoveAnimParam, _horizontalMove);
        _animator.SetFloat(VertMoveAnimParam, _verticalMove);
        
        _animator.SetFloat(SpeedAnimParam, _rigidbody2D.velocity.sqrMagnitude);
    }

    private void SetMovement()
    {
        _rigidbody2D.velocity = new Vector2(_horizontalMove, _verticalMove) * speed;
    }
}
