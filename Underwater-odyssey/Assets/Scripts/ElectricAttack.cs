using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricAttack : MonoBehaviour
{
    public GameObject electricBallPrefab;  // The electric ball projectile prefab
    public Transform firePoint;            // A point from where the electric ball will be fired
    public float shootCooldown = 5f;       // Time between electric ball attacks
    public float projectileSpeed = 5f;     // Speed of the electric ball projectile

    private GameObject player;             // Reference to the player
    private float shootTimer = 0f;         // Timer for shooting

    void Start()
    {
        // Find the player by tag
        player = GameObject.FindWithTag("Player");
    }

    void Update()
    {
        if (player != null)
        {
            // Increment the timer
            shootTimer += Time.deltaTime;

            // Check if it's time to shoot
            if (shootTimer >= shootCooldown)
            {
                ShootElectricBall();
                shootTimer = 0f;  // Reset the timer after shooting
            }
        }
    }

    // Function to shoot electric ball towards the player
    void ShootElectricBall()
    {
        // Instantiate the electric ball at the fire point and set its direction towards the player
        GameObject electricBall = Instantiate(electricBallPrefab, firePoint.position, firePoint.rotation);

        // Assuming the electric ball has a Rigidbody2D for movement
        Rigidbody2D electricBallRb = electricBall.GetComponent<Rigidbody2D>();
        Vector2 direction = (player.transform.position - firePoint.position).normalized;
        electricBallRb.velocity = direction * projectileSpeed; // Shoot towards the player
    }
}