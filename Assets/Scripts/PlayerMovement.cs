using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public FloatingJoystick joystick;

    public float speed;
    private Rigidbody rb;
    public bool CanMove;
    public Vector3 movementDirection;
    private Animator _animator;

    private float _movementSpeed;

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
        {
            _animator.SetBool("IsMoving", false);
            rb.linearVelocity = new Vector3(joystick.Horizontal * 0, rb.linearVelocity.y, joystick.Vertical * 0);
            movementDirection = new Vector3(0, 0, 0);
            movementDirection.Normalize();
        }

    }


    public void BlockMoving()
    {
        if (CanMove)
        {
            CanMove = false;
        }
        else
        {
            CanMove = true;
        }
    }    


    void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {

        }
    }
}
