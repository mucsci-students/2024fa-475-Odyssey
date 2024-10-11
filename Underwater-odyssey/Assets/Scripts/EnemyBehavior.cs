// Underwater Odyssey
// Enemy Behavior (Movement) Script
// Tim King
// Modified: 10/07/2024


using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyBehavior : MonoBehaviour
{
    // VARIABLES
    public float moveSpeed = 5f;
    public float wakeDistace = 22f;


    private Rigidbody2D rb;
    private GameObject player;  // Reference to the player GameObject
    private bool awake;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0; // Disable default gravity
        rb.freezeRotation = true; // Prevents enemy from spinning upon collision
        gameObject.tag = "Enemy";
        player = GameObject.FindWithTag("Player");
        awake = false;
    }


    // Update is called once per frame
    void Update()
    {
        if (!awake) {
            checkAwake();
        } else {
            // Determine the horizontal movement direction based on the enemy's position relative to the player
            float horizontalDirection = (transform.position.x < player.transform.position.x) ? moveSpeed : -moveSpeed;


            // Determine the vertical movement direction based on the enemy's position relative to the player
            float verticalDirection = (transform.position.y < player.transform.position.y) ? moveSpeed : -moveSpeed;
           
            // Apply movement to the enemy
            Vector2 movement = new Vector2(horizontalDirection, verticalDirection) * Time.deltaTime;
            rb.MovePosition(rb.position + movement);
        }
    }


    // Checks if the enemy has been awaken by the player
    void checkAwake()
    {
        // Calculate the distance between the enemy and the player
        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);


        // Check if the distance is less than or equal to wakeDistance
        if (distanceToPlayer <= wakeDistace)
        {
            awake = true;  // Wake the enemy
        }
    }
}
