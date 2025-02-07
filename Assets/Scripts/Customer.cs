using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public string target;
    public CustomersMeneger meneger;
    public int BuyObjCount;
    public SellStation st;
    private ArcadeMachine machine;

    public bool play, toilet;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (target == "Buy")
        {
            targetPos = st.CustomerPlace[GetPosinQueue()];
            OrderCount.text = BuyObjCount.ToString();
            //StartCoroutine(MakeCalll());
            CallTime = 200;
            step = CallTime;
        }
        if(target == "Play")
        {
            targetPos = machine.CustomerPos.transform;
        }

    }


    void Update()
    {
        if(OrderBar != null) { OrderBar.transform.LookAt(Camera.main.transform); }
        agent.SetDestination(targetPos.position);

        if (target == "Buy")
        {
            targetPos = st.CustomerPlace[GetPosinQueue()];
            if (transform.position.x == targetPos.position.x && transform.position.z == targetPos.position.z && targetPos == st.CustomerPlace[0])
            {
                st.client = this;
                st.CountObj = BuyObjCount;
            }
        }
        else if(target == "Play")
        {
            if (transform.position.x == targetPos.position.x && transform.position.z == targetPos.position.z)
            {
                machine.client = this;
                if (!machine.isPlaying && machine.CanWork)
                {
                    machine.StartPlay();
                }
            }
        }
        if (transform.position.x == DeletePlace.position.x && transform.position.z == DeletePlace.position.z) { Destroy(gameObject); }
    }

    public void ChangeBuyCount()
    {
        BuyObjCount--;
        OrderCount.text = BuyObjCount.ToString();
        if(BuyObjCount == 0)
        {
            Destroy(OrderBar);
            Destroy(OrderCount);
            GoAway();
        }
    }

    public void GoAway()
    {
        if (!play)
        {
            int a = 0;
            for (int i = 0; i < meneger.ArcadeMachines.Length; i++)
            {
                if (meneger.ArcadeMachines[i].GetComponent<ArcadeMachine>().CanWork && !meneger.ArcadeMachines[i].GetComponent<ArcadeMachine>().hasTarget)
                {
                    a++;
                }
            }
            if (a > 0)
            {
                for (int i = 0; i < meneger.ArcadeMachines.Length; i++)
                {
                    if (meneger.ArcadeMachines[i].GetComponent<ArcadeMachine>().CanWork && !meneger.ArcadeMachines[i].GetComponent<ArcadeMachine>().hasTarget)
                    {
                        meneger.ArcadeMachines[i].GetComponent<ArcadeMachine>().hasTarget = true;
                        machine = meneger.ArcadeMachines[i].GetComponent<ArcadeMachine>();
                        target = "Play";
                        targetPos = machine.CustomerPos.transform;
                        st.QueueList.Remove(gameObject);
                        st.Queue = st.QueueList.ToArray();
                        break;
                    }
                }
            }
            else
            {
                target = "Delete";
                targetPos = DeletePlace.transform;
                st.QueueList.Remove(gameObject);
                st.Queue = st.QueueList.ToArray();
            }
        }
        else if(play && !toilet)
        {
            int a = 0;
            for (int i = 0; i < meneger.Toilets.Length; i++)
            {
                if (meneger.Toilets[i].GetComponent<ArcadeMachine>().CanWork && !meneger.Toilets[i].GetComponent<ArcadeMachine>().hasTarget)
                {
                    a++;
                }
            }
            if (a > 0)
            {
                for (int i = 0; i < meneger.Toilets.Length; i++)
                {
                    if (meneger.Toilets[i].GetComponent<ArcadeMachine>().CanWork && !meneger.Toilets[i].GetComponent<ArcadeMachine>().hasTarget)
                    {
                        meneger.Toilets[i].GetComponent<ArcadeMachine>().hasTarget = true;
                        machine = meneger.Toilets[i].GetComponent<ArcadeMachine>();
                        target = "Play";
                        targetPos = machine.CustomerPos.transform;
                        st.QueueList.Remove(gameObject);
                        st.Queue = st.QueueList.ToArray();
                        break;
                    }
                }
            }
            else
            {
                target = "Delete";
                targetPos = DeletePlace.transform;
                st.QueueList.Remove(gameObject);
                st.Queue = st.QueueList.ToArray();
            }
        }

        else
        {

            target = "Delete";
            targetPos = DeletePlace.transform;
            st.QueueList.Remove(gameObject);
            st.Queue = st.QueueList.ToArray();
        }
    }

    public int GetPosinQueue()
    {
        int a = 0;

        for (int i = 0; i < st.Queue.Length; i++)
        {
            if (st.Queue[i] == gameObject)
            {
                a = i;
                break;
            }
        }
        return a;
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
