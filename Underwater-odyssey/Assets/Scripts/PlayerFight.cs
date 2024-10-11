// Underwater Odyssey
// Player Fight Script
// Tim King
// Modified: 10/07/2024


using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerFight : MonoBehaviour
{
    public float health;
    public float damage;


    private Rigidbody2D rb;
    private bool inContact;
    private GameObject currentEnemy;  // Store reference to the current enemy


    public GameObject attackParticlesPrefab;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        inContact = false;
        currentEnemy = null;
    }


    // Update is called once per frame
    void Update()
    {
        if (inContact && Input.GetMouseButtonDown(0)) // 0 is the left mouse button
        {
            Attack();
        }
    }


    // Detect collision when entering contact with an enemy
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            inContact = true; // Set the boolean to true when colliding with an enemy
            currentEnemy = collision.gameObject;
        }
    }


    // Detect when the player exits contact with the enemy
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            inContact = false; // Set the boolean to false when leaving the enemy
            currentEnemy = null;
        }
    }


    void Attack() {
        // Instantiate the particle system at the player's position and rotation
        GameObject particles = Instantiate(attackParticlesPrefab, transform.position, transform.rotation);
         
        // Destroy the particle system after 1 second
        Destroy(particles, 1.0f);


        // Retrieve the EnemyBehavior component from the current enemy
        EnemyFight enemy = currentEnemy.GetComponent<EnemyFight>();
        enemy.health -= damage;
    }
}

