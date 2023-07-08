using DefaultNamespace;
using UnityEngine;

public class PlayerController : MonoBehaviour, IPlayerController
{
    public float moveSpeed = 1f; // Speed of the player movement

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

        if (moveX != 0f && moveY != 0f)
        {
            movement *= 0.7f;
        }
        
        // Normalize movement vector to ensure constant speed regardless of direction
        movement = movement * moveSpeed;
        
        // Move the player
        rb.velocity = movement;
        // rb.MovePosition(rb.position + movement * Time.deltaTime);
    }
}