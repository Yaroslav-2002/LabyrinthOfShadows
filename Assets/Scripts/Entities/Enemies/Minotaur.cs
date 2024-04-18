using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minotaur : EntityController
{
    [SerializeField] Animator animator;
    [SerializeField] Rigidbody rb;
    [SerializeField] float speed;
    private static readonly int HorizMoveAnimParam = Animator.StringToHash("HorizontalMove");
    private static readonly int VertMoveAnimParam = Animator.StringToHash("VerticalMove");
    private static readonly int SpeedAnimParam = Animator.StringToHash("Speed");

    void Start()
    {
        
    }

    //private void SetAnimation()
    //{
    //    animator.SetFloat(HorizMoveAnimParam, _horizontalMove);
    //    animator.SetFloat(VertMoveAnimParam, _verticalMove);

    //    animator.SetFloat(SpeedAnimParam, rb.velocity.sqrMagnitude);
    //}

    //private void Move(Vector2 direction)
    //{
    //    rb.velocity = direction * speed;

       
    //}


    //void Update()
    //{
    //    SetAnimation();
    //    Move();
    //}
}
