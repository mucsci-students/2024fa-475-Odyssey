using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject harpoonPrefab;
    public Transform firepoint;
    public float fireForce = 20f;

    public float rotationSpeed = 10f; // Adjust this for sensitivity

    private void Update()
    {
        Aim();
    }

    public void Fire()
    {
        GameObject harpoon = Instantiate(harpoonPrefab, firepoint.position, firepoint.rotation);
        harpoon.GetComponent<Rigidbody2D>().AddForce(firepoint.up * fireForce, ForceMode2D.Impulse);
    }

    private void Aim()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePosition - transform.position).normalized;

        // Calculate the angle based on direction
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Correct the angle based on the player's facing direction
        if (transform.parent.localScale.x < 0) // Check if the player is facing left
        {
            targetAngle += 180; // Flip the angle if the player is facing left
        }

        // Smoothly rotate towards the target angle
        float angle = Mathf.LerpAngle(transform.eulerAngles.z, targetAngle, Time.deltaTime * rotationSpeed);
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}