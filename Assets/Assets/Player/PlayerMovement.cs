using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float forwardSpeed = 5f; // Initial forward speed
    public float acceleration = 0.1f; // Rate of acceleration
    public float jumpForce = 7f; // Upward force applied when jumping
    public float maxForwardSpeed = 20f; // Maximum forward speed
    public float maxHeight = 25f; // Maximum height the player can reach
    public float minHeight = -3f; // Minimum height the player can reach

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
       
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began && transform.position.y < maxHeight)
            {
                rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
            }
        }
    }

    void FixedUpdate()
    {
        forwardSpeed = Mathf.Min(forwardSpeed + acceleration * Time.fixedDeltaTime, maxForwardSpeed);
        
        rb.velocity = new Vector3(forwardSpeed, rb.velocity.y, rb.velocity.z);
        
        Vector3 clampedPosition = transform.position;
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, minHeight, maxHeight);
        transform.position = clampedPosition;
    }
}
