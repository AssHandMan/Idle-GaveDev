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
    private Animator _animator;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        CanMove = true;
        _animator = GetComponent<Animator>();
        _animator.applyRootMotion = false;
    }

    void Update()
    {
        if (CanMove)
        {
            rb.linearVelocity = new Vector3(joystick.Horizontal * speed, rb.linearVelocity.y, joystick.Vertical * speed);

            movementDirection = new Vector3(joystick.Horizontal, 0, joystick.Vertical);
            movementDirection.Normalize();
            if (movementDirection != Vector3.zero) 
            { 
                transform.forward = movementDirection;
                _animator.SetBool("IsMoving", true);
            }
            else
            {
                _animator.SetBool("IsMoving", false);
            }
        }
        else
            _animator.SetBool("IsMoving", false);
    }


    void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {

        }
    }
}
