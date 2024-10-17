// Underwater Odyssey
// Enemy Fight Script
// Tim King
// Modified: 10/07/2024

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

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
    public Gun gun;
    public int harpoonAmount = 5;

    private Rigidbody2D rb;
    Vector2 movement;
    Vector2 mouse;
    private bool facingRight = true;
    private Animator anim;

    private bool inContact;
    private GameObject currentEnemy;  // Store reference to the current enemy
    public FloatingHealthBar healthBar;
    public int coinCount = 0;
    public TextMeshProUGUI coinText;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0; // Disable default gravity
        gameObject.tag = "Player";
        inContact = false; // Initially not in contact with any enemy
        UpdateCoinUI();
        UpdateHealthBar();
    }

    void Update()
    {
    mouse = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
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
        UpdateHealthBar();
        if (currentHealth <= 0)
        {
            HandleDeath();
        }
    }

    public void CollectCoin()
    {
        coinCount++;
        UpdateCoinUI();
    }

    public void increaseHarpoon(){
        harpoonAmount++;
    }

    void UpdateCoinUI()
    {
        coinText.text = "Coins: x" + coinCount.ToString();
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
        Vector2 aimDir = mouse - rb.position;    
        float aimAngle = Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg - 90f; 
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
        else if (Input.GetMouseButtonDown(0)){
            if (harpoonAmount > 0){
                gun.Fire();
                harpoonAmount--;
            }
            
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

    private void UpdateHealthBar()
    {
        healthBar.UpdateHealthBar(currentHealth, maxHealth); // Update health bar
    }
}