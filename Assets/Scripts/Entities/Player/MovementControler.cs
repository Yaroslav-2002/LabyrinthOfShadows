using Controls;
using UnityEngine;
using UnityEngine.InputSystem.XInput;

public class MovementController : MonoBehaviour
{
    private Rigidbody2D rigidBody;
    [SerializeField] private float speed;

    private IInputController _inputController;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();

        _inputController =
#if UNITY_EDITOR
    new KeyBoardInputController();
#else 
        new JoystickInputController(dynamicJoystick);
#endif 
    }

    private void FixedUpdate()
    {
        float horizontalMove = _inputController.GetHorizontal();
        float verticalMove = _inputController.GetVertical();

        Vector2 movement = new Vector2(horizontalMove, verticalMove) * speed;
        rigidBody.velocity = movement;
    }
}