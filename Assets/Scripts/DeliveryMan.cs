using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class DeliveryMan : MonoBehaviour
{
    public Transform place, delete, target;
    public int BuyObjCount;
    public GameObject OrderBar;
    public Text OrderCount;
    public string Type;
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        OrderCount.text = BuyObjCount.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (BuyObjCount > 0)
        {
            OrderBar.transform.LookAt(Camera.main.transform);
            target = place;
        }
        agent.SetDestination(target.position);

        if(transform.position == delete.position) { Destroy(gameObject); }
    }

    public void AddToOrder()
    {
        if (BuyObjCount > 0)
        {
            BuyObjCount--;
            OrderCount.text = BuyObjCount.ToString();
        }
        if (BuyObjCount == 0)
        {
            Destroy(OrderBar);
            Destroy(OrderCount);
            target = delete;
        }
    }
}
