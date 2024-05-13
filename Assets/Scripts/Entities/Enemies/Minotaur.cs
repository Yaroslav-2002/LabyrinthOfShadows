using System;
using UnityEngine;

[Serializable]
public class Minotaur : EntityController
{
    [SerializeField] Animator animator;
    [SerializeField] Rigidbody rb;
    [SerializeField] float speed;

    private static readonly int HorizMoveAnimParam = Animator.StringToHash("HorizontalMove");
    private static readonly int VertMoveAnimParam = Animator.StringToHash("VerticalMove");
    private static readonly int SpeedAnimParam = Animator.StringToHash("Speed");

    public Minotaur() : base()
    {
        
    }
}
