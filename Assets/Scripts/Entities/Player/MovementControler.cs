using UnityEngine;
using UnityEngine.InputSystem;

public class MovementController : MonoBehaviour
{
    [SerializeField] private float speed;

    private Rigidbody2D _rigidBody;
    private PlayerInput _playerInput;
    private InputAction _buttonsAction;
    private Vector2 _playerInputVector;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _playerInput = GetComponent<PlayerInput>();

        // Find the specific actions directly
        _buttonsAction = _playerInput.actions["Buttons"];

        // Register event handlers for actions
       
        _buttonsAction.performed += ButtonClicked;
        _buttonsAction.canceled += ButtonClicked;
    }

    private void OnEnable()
    {
        _buttonsAction.Enable();
    }

    private void OnDisable()
    {
        _buttonsAction.Disable();
    }

    private void ButtonClicked(InputAction.CallbackContext context)
    {
        _playerInputVector = context.ReadValue<Vector2>();
        //Debug.Log("Button clicked " + context.ReadValue<Vector2>());
    }

    private void FixedUpdate()
    {
        Vector2 movement = _playerInputVector * speed;
        _rigidBody.velocity = movement;
    }
}
