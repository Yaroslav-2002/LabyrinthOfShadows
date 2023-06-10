using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f; // Speed of the player movement

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Get input axes
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        // Create movement vector
        Vector2 movement = new Vector2(moveX, moveY);

        // Normalize movement vector to ensure constant speed regardless of direction
        movement = movement.normalized * moveSpeed;

        // Move the player
        rb.MovePosition(rb.position + movement * Time.deltaTime);
    }
}