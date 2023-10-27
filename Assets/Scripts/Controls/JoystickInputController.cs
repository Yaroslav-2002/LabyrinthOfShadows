using System;

namespace Controls
{
    public class JoystickInputController : InputController
    {
        private DynamicJoystick _joystick;

        public JoystickInputController(DynamicJoystick dynamicJoystick)
        {
            _joystick = dynamicJoystick;
        }

        public override float GetHorizontal()
        {
            return _joystick.Horizontal;
        }

        public override float GetVertical()
        {
            return _joystick.Vertical;
        }
    }
}