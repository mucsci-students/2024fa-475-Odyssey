// Underwater Odyssey
// Player Behavior (Movement) Script
// Tim King
// Modified: 9/30/2024

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behavior : MonoBehaviour
{
    // VARIABLES
    public float moveSpeed = 5f;     // Horizontal movement speed
    public float buoyancy = 2f;      // Force that simulates upward movement in water
    public float sinkingSpeed = 0.5f; // Speed of falling when no input is given
    public LayerMask groundLayer;    // Layer mask to define what is considered ground

    public float health;

    private Rigidbody2D rb;
    private Vector2 movement;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0; // Disable default gravity
        gameObject.tag = "Player";
    }

    void Update()
    {
        // Get input from WASD or arrow keys
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // Check if the player is on the ground
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 0.1f, groundLayer);

        // If the player presses the "up" key or is already moving upward
        if (movement.y > 0)
        {
            // Allow upward movement even if touching the ground
            rb.velocity = new Vector2(rb.velocity.x, buoyancy * moveSpeed);
        }
        else if (movement.y < 0)
        {
            // Allow downward movement as long as it's not grounded
            rb.velocity = new Vector2(rb.velocity.x, -moveSpeed);
        }
        else if (!isGrounded)
        {
            // Apply slow sinking when there is no input and player is not grounded
            rb.velocity = new Vector2(rb.velocity.x, -sinkingSpeed);
        }
        else
        {
            // Stop sinking if grounded to avoid getting stuck at the bottom
            rb.velocity = new Vector2(rb.velocity.x, 0);
        }
    }

    void FixedUpdate()
    {
        // Apply horizontal movement
        rb.velocity = new Vector2(movement.x * moveSpeed, rb.velocity.y);
    }
}