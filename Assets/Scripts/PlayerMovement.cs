using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    public FloatingJoystick joystick;

    public float speed;
    private Rigidbody rb;
    public bool CanMove;
    public Vector3 movementDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        CanMove = true;
    }

    void Update()
    {
        if (CanMove)
        {
            rb.velocity = new Vector3(joystick.Horizontal * speed, rb.velocity.y, joystick.Vertical * speed);

            movementDirection = new Vector3(joystick.Horizontal, 0, joystick.Vertical);
            movementDirection.Normalize();
            if (movementDirection != Vector3.zero) 
            { 
                transform.forward = movementDirection;
 
            }
            else
            {

            }
        }
    }


    void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {

        }
    }
}
