using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricBall : MonoBehaviour
{
    private float damage = 5f;  // Default damage value

    // Handle trigger collision
    void OnTriggerEnter2D(Collider2D other)
    {
        // If the electric ball hits the player
        if (other.CompareTag("Player"))
        {
            // Retrieve the PlayerBehavior component from the player
            PlayerBehavior player = other.GetComponent<PlayerBehavior>();
            if (player != null)
            {
                player.TakeDamage(damage);  // Apply damage to the player
            }

            // Destroy the electric ball after it hits the player
            Destroy(gameObject);
        }
        // Ignore collisions with the shooter or other enemies
        else if (other.CompareTag("Enemy"))
        {
            // Do nothing, just ignore the collision
        }
    }
}