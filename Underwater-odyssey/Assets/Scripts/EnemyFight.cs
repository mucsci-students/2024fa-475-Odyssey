// Underwater Odyssey
// Enemy Fight Script
// Tim King
// Modified: 10/07/2024


using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyFight : MonoBehaviour
{
    public float health;
    public float damage;


    private Rigidbody2D rb;
    private bool inContact;
    private float cooldown;
    private GameObject player;  // Store reference to the player


    public GameObject attackParticlesPrefab;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        inContact = false;
        player = null;
        cooldown = 0f;
    }


    // Update is called once per frame
    void Update()
    {
        // Decrease the cooldown over time
        if (cooldown > 0)
        {
            cooldown -= Time.deltaTime;  // Reduce cooldown by the time passed since the last frame
        }


        // Sends signal to attack player if in contact and has no cooldown
        if (inContact && cooldown <= 0f)
        {
            Attack();
        }
    }


    // Detect collision when entering contact with an enemy
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            inContact = true; // Set the boolean to true when colliding with an enemy
            player = collision.gameObject;
        }
    }


    // Detect when the player exits contact with the enemy
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            inContact = false; // Set the boolean to false when leaving the enemy
            player = null;
        }
    }


    void Attack() {
        // Instantiate the particle system at the player's position and rotation
        GameObject particles = Instantiate(attackParticlesPrefab, transform.position, transform.rotation);
         
        // Destroy the particle system after 1 second
        Destroy(particles, 1.0f);


        // Retrieve the PlayerFight component from the player
        PlayerFight target = player.GetComponent<PlayerFight>();
        target.health -= damage;
        cooldown = 2f;
    }
}