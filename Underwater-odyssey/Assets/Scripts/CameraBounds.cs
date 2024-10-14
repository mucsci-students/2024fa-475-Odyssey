using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBounds : MonoBehaviour
{
    public GameObject cameraBounds;    
    
    private Collider2D boundsCollider; 
    private Camera cam;               

    private float minX, maxX, minY, maxY; // Boundaries

    private void Start()
    {
        cam = Camera.main;

        if (cameraBounds != null)
        {
            boundsCollider = cameraBounds.GetComponent<Collider2D>();

            if (boundsCollider != null)
            {
                CalculateBounds();
            }
            else
            {
                Debug.LogError("CameraBounds GameObject does not have a Collider2D component!");
            }
        }
        else
        {
            Debug.LogError("CameraBounds is not assigned in the Inspector.");
        }
    }

    private void LateUpdate()
    {
        // Constrain the camera's position within the bounds
        Vector3 clampedPosition = KeepCameraWithinBounds(transform.position);
        transform.position = clampedPosition;
    }

    void CalculateBounds()
    {
        if (cam.orthographic)
        {
            Bounds bounds = boundsCollider.bounds;

            float camHeight = cam.orthographicSize;
            float camWidth = cam.aspect * camHeight;

            minX = bounds.min.x + camWidth;
            maxX = bounds.max.x - camWidth;
            minY = bounds.min.y + camHeight;
            maxY = bounds.max.y - camHeight;
        }
        else
        {
            Debug.LogError("This script is designed for an orthographic camera.");
        }
    }

    Vector3 KeepCameraWithinBounds(Vector3 position)
    {
        if (minX < maxX && minY < maxY)
        {
            position.x = Mathf.Clamp(position.x, minX, maxX);
            position.y = Mathf.Clamp(position.y, minY, maxY);
        }
        else
        {
            Debug.LogWarning("Bounds are not properly calculated. Ensure your camera bounds are larger than the camera's viewport.");
        }

        return position;
    }
}
