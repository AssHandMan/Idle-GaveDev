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
        OrderBar.SetActive(false);
        OrderCount.text = BuyObjCount.ToString();
        target = place;
    }

    // Update is called once per frame
    void Update()
    {

        agent.SetDestination(target.position);
        if (transform.position.x == target.position.x && transform.position.z == target.position.z) 
        {
            if (!OrderBar.activeSelf) { OrderBar.SetActive(true); } 
            if (BuyObjCount > 0) { OrderBar.transform.LookAt(Camera.main.transform); }
        }

        if (transform.position.x == delete.position.x && transform.position.z == delete.position.z) { Destroy(gameObject); }
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
            OrderBar.SetActive(false);
            target = delete;
        }
    }
}
