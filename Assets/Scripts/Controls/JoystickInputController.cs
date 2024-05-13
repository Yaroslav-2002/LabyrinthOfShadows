namespace Controls
{
    public class JoystickInputController : IInputController
    {
        private DynamicJoystick _joystick;

        public JoystickInputController(DynamicJoystick dynamicJoystick)
        {
            _joystick = dynamicJoystick;
        }

        public float GetHorizontal()
        {
            return _joystick.Horizontal;
        }

        public float GetVertical()
        {
            return _joystick.Vertical;
        }
    }
}