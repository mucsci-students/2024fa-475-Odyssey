using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harpoon : MonoBehaviour
{
    public float damage = 15f;
    private bool inContact;
    private GameObject currentEnemy;
    private GameObject player;
    private float cooldown = 0.5f; // Reasonable cooldown duration
    private float lastDamageTime;
    private bool didDamage = false;

    private float lifespan = 2f; // Minimum lifespan before destruction
    private float spawnTime; // Time when the harpoon was launched

    void Start()
    {
        // Record the time when the harpoon was created
        spawnTime = Time.time;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            inContact = true; // Set to true when colliding with an enemy
            currentEnemy = collision.gameObject;
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            player = collision.gameObject;
            // Check if enough time has passed before destroying the harpoon
            if (Time.time - spawnTime >= lifespan)
            {   
                PlayerBehavior curPlayer = player.GetComponent<PlayerBehavior>();
                curPlayer.increaseHarpoon();
                Destroy(gameObject); // Destroy the harpoon when the player comes into contact
            }
        }
    }

    void Update()
    {
        if (inContact && Time.time > lastDamageTime + cooldown && !didDamage) // Check if enough time has passed
        {
            EnemyBehavior enemy = currentEnemy.GetComponent<EnemyBehavior>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage); // Apply damage to the enemy
                didDamage = true;
                lastDamageTime = Time.time; // Update the last damage time
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            inContact = false; // Set to false when leaving the enemy
            currentEnemy = null;
        }
    }
}