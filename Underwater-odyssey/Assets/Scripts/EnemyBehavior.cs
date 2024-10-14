// Underwater Odyssey
// Enemy Fight Script
// Tim King
// Modified: 10/07/2024

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
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
    private bool awake;
    private bool inContact;
    public FloatingHealthBar healthBar; // Reference to the health bar

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0; // Disable default gravity
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
        else
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
        Vector2 direction = (player.transform.position - transform.position).normalized;
        rb.MovePosition(rb.position + direction * moveSpeed * Time.deltaTime);
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        healthBar.UpdateHealthBar(currentHealth, maxHealth); // Update health bar
        if (currentHealth <= 0){
            HandleDeath();
        }
        
    }

    private void HandleDeath()
    {
        Destroy(gameObject); // Destroy the enemy object
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            inContact = true; // Set the boolean to true when colliding with the player
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
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
            target.currentHealth -= damage; // Apply damage to the player
        }
        
        cooldown = 2f; // Reset cooldown
    }
}