using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundEnemyBehavior : MonoBehaviour
{
    // Movement Variables
    public float moveSpeed = 5f;
    public float wakeDistance = 22f;
    public float maxHealth = 10f;
    public float currentHealth;
    public float damage = 2f; // Damage dealt to the player

    // Attack Variables
    public GameObject attackParticlesPrefab;
    private float cooldown = 2f; // Time between attacks

    private Rigidbody2D rb;
    private GameObject player;  // Reference to the player GameObject
    private bool awake;         // Whether the enemy is awake
    private bool inContact;     // Whether the enemy is in contact with the player
    public FloatingHealthBar healthBar; // Reference to the health bar

    private bool facingRight = false;   // Tracks which way the enemy is facing
    private bool isGrounded = false;    // Checks if the enemy is on the ground

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 1; // Enable gravity so the enemy stays on the ground
        rb.freezeRotation = true; // Prevents enemy from spinning upon collision
        gameObject.tag = "Enemy";
        player = GameObject.FindWithTag("Player");
        awake = false;
        inContact = false;
        currentHealth = maxHealth; // Initialize current health
        healthBar.UpdateHealthBar(currentHealth, maxHealth); // Initialize health bar
    }

    void Update()
    {
        if (!awake)
        {
            CheckAwake();
        }
        else if (isGrounded) // Only move if the enemy is grounded
        {
            MoveTowardsPlayer();
            if (inContact && cooldown <= 0f)
            {
                Attack();
            }

            // Reduce cooldown
            if (cooldown > 0)
            {
                cooldown -= Time.deltaTime;  // Decrease cooldown over time
            }
        }

        // Check for enemy death
        if (currentHealth <= 0)
        {
            HandleDeath();
        }
    }

    // Flip the character's sprite when changing direction
    void FlipCharacter()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1; // Flip the sprite by inverting the x scale
        transform.localScale = scale;
    }

    void CheckAwake()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
        if (distanceToPlayer <= wakeDistance)
        {
            healthBar = GetComponentInChildren<FloatingHealthBar>();
            awake = true;  // Wake the enemy
        }
    }

    void MoveTowardsPlayer()
    {
        Vector2 direction = (player.transform.position - transform.position);
        direction.y = 0; // Only move horizontally

        rb.MovePosition(rb.position + direction.normalized * moveSpeed * Time.deltaTime);

        // Flip the character if needed based on movement direction
        if (direction.x > 0 && !facingRight)
        {
            FlipCharacter();
        }
        else if (direction.x < 0 && facingRight)
        {
            FlipCharacter();
        }
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        healthBar.UpdateHealthBar(currentHealth, maxHealth); // Update health bar
        if (currentHealth <= 0)
        {
            HandleDeath();
        }
    }

    private void HandleDeath()
    {
        Destroy(gameObject); // Destroy the enemy object
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the enemy is on the ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true; // The enemy is grounded
        }

        // Handle collision with player for attack
        if (collision.gameObject.CompareTag("Player"))
        {
            inContact = true; // Set the boolean to true when colliding with the player
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        // Check if the enemy has left the ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false; // The enemy is no longer grounded
        }

        // Handle exiting collision with the player
        if (collision.gameObject.CompareTag("Player"))
        {
            inContact = false; // Set the boolean to false when leaving the enemy
        }
    }

    void Attack()
    {
        GameObject particles = Instantiate(attackParticlesPrefab, transform.position, transform.rotation);
        Destroy(particles, 1.0f); // Destroy the particle system after 1 second

        // Retrieve the PlayerBehavior component from the player
        PlayerBehavior target = player.GetComponent<PlayerBehavior>();
        if (target != null)
        {
            target.TakeDamage(damage); // Apply damage to the player
        }

        cooldown = 2f; // Reset cooldown
    }
}