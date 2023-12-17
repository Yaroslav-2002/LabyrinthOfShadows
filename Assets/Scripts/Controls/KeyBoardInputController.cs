using UnityEngine;

namespace Controls
{
    public class KeyBoardInputController : InputController
    {
        public override float GetHorizontal()
        {
            if (Input.GetKey(KeyCode.A))
                return -1;
            if(Input.GetKey(KeyCode.D)) 
            {
                return 1;
            }
            return 0;
        }

        public override float GetVertical()
        {
            if (Input.GetKey(KeyCode.W))
                return 1;
            return Input.GetKey(KeyCode.S) ? -1 : 0;
        }
    }
}