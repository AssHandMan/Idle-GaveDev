using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class DoorSlider : MonoBehaviour
{
    private Animator anim;
    private GameObject plr;
    private List<GameObject> Menegers = new List<GameObject>();
    private List<GameObject> Customers = new List<GameObject>();
    private bool isClosed;
    void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("Close", true);
        isClosed = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(!isClosed)
        {
            if (plr == null && Menegers.Count == 0 && Customers.Count == 0)
            {
                anim.SetBool("Close", true);
                isClosed = true;
            }
        }
        else
        {
            if (plr != null || Menegers.Count > 0 || Customers.Count > 0)
            {
                anim.SetBool("Close", false);
                isClosed = false;
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
       if(other.GetComponent<PlayerMovement>() != null)
       {
            plr = other.gameObject;
       }
       if(other.GetComponent<Meneger>() != null)
       {
            Menegers.Add(other.gameObject);
       }
       if(other.GetComponent<Customer>() != null)
       {
            Customers.Add(other.gameObject);
       }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerMovement>() != null)
        {
            plr = null;
        }
        if (other.GetComponent<Meneger>() != null)
        {
            Menegers.Remove(other.gameObject);
        }
        if (other.GetComponent<Customer>() != null)
        {
            Customers.Remove(other.gameObject);
        }
    }
}
