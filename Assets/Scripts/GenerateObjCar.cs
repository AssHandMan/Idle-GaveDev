using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateObjCar : MonoBehaviour
{
    public float speed;
    public bool CanMove, isNewUdpate, CanGive;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        CanMove = true;
        CanGive = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isNewUdpate)
        {
            if (speed < 0) { speed *= -2; }
            rb.linearVelocity = new Vector3(0f, 0f, speed);
            CanGive = false;
        }
        else
        {
            if (CanMove) { rb.linearVelocity = new Vector3(0f, 0f, speed); }
            else { rb.linearVelocity = new Vector3(0f, 0f, 0); }

            if (transform.position.z > 100) { Destroy(gameObject); }
        }
    }

    public void TurnAway()
    {
        CanMove = true;
        speed *= -2;
    }
}
