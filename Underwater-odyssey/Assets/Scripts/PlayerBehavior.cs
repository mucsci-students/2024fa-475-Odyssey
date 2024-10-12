// Underwater Odyssey
// Player Behavior (Movement) Script
// Tim King
// Modified: 10/10/2024


using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Behavior : MonoBehaviour
{
    // VARIABLES
    public float moveSpeed = 5f;     // Horizontal movement speed
    public float buoyancy = 2f;      // Force that simulates upward movement in water
    public float sinkingSpeed = 0.5f; // Speed of falling when no input is given


    private Rigidbody2D rb;
    private Vector2 movement;


    private bool facingRight = true;
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0; // Disable default gravity
        gameObject.tag = "Player";
    }



    void Update()
    {
        // Get input from WASD or arrow keys
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");


        // If the player presses the "up" key or is already moving upward
        if(!PauseMenu.isPaused){
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
        else
        {
            // Apply slow sinking when there is no input and player is not grounded
             rb.velocity = new Vector2(rb.velocity.x, -sinkingSpeed);
        }




        //Flp character based on movement
        if (movement.x > 0 && !facingRight)
        {
            FlipCharacter();
        }
        else if (movement.x < 0 && facingRight)
        {
            FlipCharacter();
        }
        anim.SetInteger("Speed",Mathf.Abs((int)rb.velocity.x));
        }
    }


    // Changes player orientation
    private void FlipCharacter()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }


    void FixedUpdate()
    {
        // Apply horizontal movement
        rb.velocity = new Vector2(movement.x * moveSpeed, rb.velocity.y);
    }
}