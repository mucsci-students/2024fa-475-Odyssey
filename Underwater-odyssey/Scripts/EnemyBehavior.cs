// Underwater Odyssey
// Enemy Behavior (Movement) Script
// Tim King
// Modified: 9/30/2024

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    // VARIABLES
    public float moveSpeed = 5f;
    public float verticalSpeed = 2f;  // Speed of random vertical movement
    public float verticalChangeInterval = 1f;
    public LayerMask groundLayer;

    public float health;

    private Rigidbody2D rb;
    private float randomYDirection;
    private float timer;
    private bool isGrounded;

    private GameObject player;  // Reference to the player GameObject

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0; // Disable default gravity
        SetRandomYDirection();
        gameObject.tag = "Enemy";
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        // Determine the horizontal movement direction based on the enemy's position relative to the player
        float horizontalDirection = (transform.position.x < player.transform.position.x) ? moveSpeed : -moveSpeed;

        // Check if the enemy is grounded
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 0.1f, groundLayer);

        // Update vertical movement only if not grounded
        if (!isGrounded)
        {
            // Apply movement to the enemy
            Vector2 movement = new Vector2(horizontalDirection, randomYDirection) * Time.deltaTime;
            rb.MovePosition(rb.position + movement);
            
            // Adjust the timer for random vertical movement
            timer += Time.deltaTime;
            if (timer >= verticalChangeInterval)
            {
                SetRandomYDirection();
                timer = 0f;
            }
        }
        else
        {
            // Stop vertical movement if grounded
            randomYDirection = 0; 
            // Apply horizontal movement even if grounded
            Vector2 movement = new Vector2(horizontalDirection, randomYDirection) * Time.deltaTime;
            rb.MovePosition(rb.position + movement);
        }
    }

    // Function to randomly change vertical movement direction
    void SetRandomYDirection()
    {
        randomYDirection = Random.Range(-verticalSpeed, verticalSpeed);  // Random up or down movement
    }

    // Collision detection to check if enemy touches the ground
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;  // Mark as grounded when it touches the ground
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;  // No longer grounded when it leaves the ground
        }
    }
}