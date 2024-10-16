using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harpoon : MonoBehaviour
{
    public float damage = 1f;
    private bool inContact;
    private GameObject currentEnemy;
    private float cooldown = 10f;


    public void OnCollisionEnter2D(Collision2D collision){
        if (collision.gameObject.CompareTag("Enemy"))
        {
            inContact = true; // Set the boolean to true when colliding with an enemy
            currentEnemy = collision.gameObject;
        }
    }
    void Update(){

        if (inContact) // Left mouse button
        {
            if (cooldown > 0)
            {
                cooldown -= Time.deltaTime;  // Decrease cooldown over time
            }
        EnemyBehavior enemy = currentEnemy.GetComponent<EnemyBehavior>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage); // Apply damage to the enemy
        }
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
}
