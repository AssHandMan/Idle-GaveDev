using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Customer : MonoBehaviour
{
    public Transform targetPos, DeletePlace;
    public GameObject OrderBar, CallBar;
    public Text OrderCount;
    public Canvas canvas;
    public bool isCalling;
    private float CallTime, step;
    private Rigidbody rb;
    private NavMeshAgent agent;
    private string[] BuyType = { "Buy", "Play" };
    private string target;

    public int BuyObjCount;
    private SellStation st;
    private ArcadeMachine machine;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        target = BuyType[UnityEngine.Random.Range(0, BuyType.Length)];
        if (target == "Buy")
        {
            st = GameObject.FindGameObjectWithTag("Cashmachine").GetComponent<SellStation>();
            targetPos = st.CustomerPlace.transform;
            BuyObjCount = UnityEngine.Random.Range(2, 5);
            OrderCount.text = BuyObjCount.ToString();
            StartCoroutine(MakeCalll());
            CallTime = 300;
            step = CallTime;
        }
        if(target == "Play")
        {
            machine = GameObject.FindGameObjectWithTag("ArcadeMachine").GetComponent<ArcadeMachine>();
            targetPos = machine.CustomerPos.transform;
        }

    }


    void Update()
    {
        if(OrderBar != null) { OrderBar.transform.LookAt(Camera.main.transform); }
        agent.SetDestination(targetPos.position);
        if (target == "Buy")
        {
            if (transform.position == targetPos.position && !isCalling)
            {
                st.client = this;
                st.CountObj = BuyObjCount;
            }
            if (BuyObjCount == 0) { targetPos = DeletePlace.transform; }
        }
        else if(target == "Play")
        {
            if (agent.transform.position == targetPos.position)
            {
                machine.client = this;
                if (!machine.isPlaying && machine.CanWork)
                {
                    machine.StartPlay();
                }
            }
        }
        if (agent.transform.position == DeletePlace.position) { Destroy(gameObject); }
    }

    public void ChangeBuyCount()
    {
        BuyObjCount--;
        OrderCount.text = BuyObjCount.ToString();
        if(BuyObjCount == 0)
        {
            Destroy(OrderBar);
            Destroy(OrderCount);
        }
    }

    public void GoAway()
    {
        targetPos = DeletePlace.transform;
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<PlayerTakeDropSystem>() != null)
        {
            if(isCalling)
            {
                StartCoroutine(Calling());
            }
        }
    }

    private IEnumerator MakeCalll()
    {
        if(!isCalling && transform.position != targetPos.position)
        {
            yield return new WaitForSeconds(1);
            if(UnityEngine.Random.Range(0,2) == 1)
            {
                agent.speed = 0;
                OrderBar.SetActive(false);
                CallBar.SetActive(true);
                isCalling = true;
            }
            else
            {
                StartCoroutine(MakeCalll());
            }
        }
    }
    private IEnumerator Calling()
    {
        if (CallTime > 0)
        {
            yield return new WaitForSeconds(0.01f);
            CallTime--;
            CallBar.GetComponent<Image>().fillAmount -= (1 / step);
            StartCoroutine(Calling());
        }
        else
        {
            agent.speed = 5;
            OrderBar.SetActive(true);
            CallBar.SetActive(false);
            isCalling = false;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerTakeDropSystem>() != null)
        {
            if (isCalling)
            {
                StopAllCoroutines();
            }
        }
    }
}
