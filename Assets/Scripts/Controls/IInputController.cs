using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Controls
{
    public abstract class InputController
    {
        protected float HorizontalMove;
        protected float VerticalMove;

        public virtual float GetHorizontal()
        {
            return HorizontalMove;
        }

        public virtual float GetVertical()
        {
            return VerticalMove;
        }
    }
}