using UnityEngine;

public class DoorSimple : MonoBehaviour
{
    private Animator anim;
    private bool isClosed;
    void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetInteger("Act", 3);
        isClosed = true;
    }


    public void OnTriggerEnter(Collider other)
    {
        if ((other.GetComponent<PlayerMovement>() != null || other.GetComponent<Customer>() != null) && isClosed)
        {
            if(other.gameObject.transform.position.x < transform.position.x)
            {
                anim.SetInteger("Act", 0);
            }
            else
            {
                anim.SetInteger("Act", 1);
            }
            isClosed = false;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if ((other.GetComponent<PlayerMovement>() != null || other.GetComponent<Customer>() != null) && !isClosed)
        {
            if (other.gameObject.transform.position.x < transform.position.x)
            {
                anim.SetInteger("Act", 2);
            }
            else
            {
                anim.SetInteger("Act", 3);
            }
            isClosed = true;
        }
    }
}