// Underwater Odyssey
// Enemy Fight Script
// Tim King
// Modified: 10/07/2024

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerBehavior : MonoBehaviour
{
    // Movement Variables
    public float moveSpeed = 5f;     
    public float buoyancy = 2f;      
    public float sinkingSpeed = 0.5f; 
    public float maxHealth = 100f;
    public float currentHealth = 100f;

    // Combat Variables
    public float damage;
    public GameObject attackParticlesPrefab;

    private Rigidbody2D rb;
    private Vector2 movement;
    private bool facingRight = true;
    private Animator anim;

    private bool inContact;
    private GameObject currentEnemy;  // Store reference to the current enemy
   // public FloatingHealthBar healthBar; // Reference to the health bar

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0; // Disable default gravity
        gameObject.tag = "Player";
        inContact = false; // Initially not in contact with any enemy
        //healthBar.UpdateHealthBar(currentHealth, maxHealth); // Initialize health bar
        
    }

    void Update()
    {
        if (!PauseMenu.isPaused)
        {
            HandleInput();
            HandleMovement();
            UpdateAnimation();
            HandleCombat();

            // Check if the player is dead
            if (currentHealth <= 0)
            {
                HandleDeath();
            }
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
       // healthBar.UpdateHealthBar(currentHealth, maxHealth); // Update health bar
        if (currentHealth <= 0)
        {
            HandleDeath();
        }
    }

    private void HandleInput()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
    }

    private void HandleMovement()
    {
        // Upward and downward movement logic
        if (movement.y > 0) rb.velocity = new Vector2(rb.velocity.x, buoyancy * moveSpeed);
        else if (movement.y < 0) rb.velocity = new Vector2(rb.velocity.x, -moveSpeed);
        else rb.velocity = new Vector2(rb.velocity.x, -sinkingSpeed);

        // Flip character based on movement direction
        if (movement.x > 0 && !facingRight) FlipCharacter();
        else if (movement.x < 0 && facingRight) FlipCharacter();

        // Apply horizontal movement
        rb.velocity = new Vector2(movement.x * moveSpeed, rb.velocity.y);
    }

    private void UpdateAnimation()
    {
        anim.SetInteger("Speed", Mathf.Abs((int)rb.velocity.x));
    }

    private void FlipCharacter()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void HandleCombat()
    {
        if (inContact && Input.GetMouseButtonDown(0)) // Left mouse button
        {
            Attack();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            inContact = true; // Set the boolean to true when colliding with an enemy
            currentEnemy = collision.gameObject;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            inContact = false; // Set the boolean to false when leaving the enemy
            currentEnemy = null;
        }
    }

    void Attack()
    {
        GameObject particles = Instantiate(attackParticlesPrefab, transform.position, transform.rotation);
        Destroy(particles, 1.0f); // Destroy the particle system after 1 second

        // Retrieve the EnemyBehavior component from the current enemy
        EnemyBehavior enemy = currentEnemy.GetComponent<EnemyBehavior>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage); // Apply damage to the enemy
        }
    }


    private void HandleDeath()
    {
        // Disable player controls
        this.enabled = false;

        // Optionally: Show Game Over screen or reset level after a delay
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex -1);
    }
}