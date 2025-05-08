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
    public GameObject OrderBar, CallIcon;
    public Image CallBar;
    public Text OrderCount;
    public Canvas canvas;
    public bool isCalling;
    public float CallTime, step;
    private Rigidbody rb;
    public NavMeshAgent agent;
    private string[] BuyType = { "Buy", "Play" };
    public string target;
    public CustomersMeneger meneger;
    public int BuyObjCount;
    public SellStation st;
    public ArcadeMachine machine;
    public GameObject[] PoorSkins, RichSkins, BloggerSkins;
    public bool play, toilet, kiosk;

    public bool CanChangePos;
    public int ppos;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        OrderBar.SetActive(false);
        if (target == "Buy")
        {
            targetPos = st.CustomerPlace[GetPosinQueue()];
            OrderCount.text = BuyObjCount.ToString();
        }

    }


    void Update()
    {
        ppos = GetPosinQueue(); 


        if (OrderBar != null) { OrderBar.transform.LookAt(Camera.main.transform); }
        if (CallIcon.activeSelf) { CallIcon.transform.LookAt(Camera.main.transform); }
        agent.SetDestination(targetPos.position);

        if (target == "Buy")
        {
            if ((st.isCalling && GetPosinQueue() > st.LustCustomerPosinQueue) || isCalling) { CanChangePos = false; }
            else { CanChangePos = true; }

            if(CanChangePos)
            {
                targetPos = st.CustomerPlace[GetPosinQueue()];
            }

            if (transform.position.x == targetPos.position.x && transform.position.z == targetPos.position.z)
            {

                if (st.isCalling)
                {
                    if (GetPosinQueue() > st.CallingCustomer.GetPosinQueue())
                    {
                        agent.speed = 0;
                    }
                }

                if (targetPos == st.CustomerPlace[0])
                {
                    if (!OrderBar.activeSelf & BuyObjCount > 0) { OrderBar.SetActive(true); }
                    st.client = this;
                    st.CountObj = BuyObjCount;
                }
            }

            if (BuyObjCount == 0)
            {
                int a = 0;
                for (int i = 0; i < meneger.ArcadeMachines.Length; i++)
                {
                    if (meneger.ArcadeMachines[i].gameObject.activeSelf)
                    {
                        if (meneger.ArcadeMachines[i].GetComponent<ArcadeMachine>().CanWork && !meneger.ArcadeMachines[i].GetComponent<ArcadeMachine>().hasTarget)
                        {
                            a++;
                        }
                    }
                }
                if (a > 0)
                {
                    st.QueueList.Remove(gameObject);
                    st.Queue = st.QueueList.ToArray();
                    GoAway();
                }
            }
        }
        else if (target == "Play")
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


    public void ChangeType(int type)
    {
        // 0 - poor
        // 1 - rich
        // 2 - bloger
        PoorSkins[0].SetActive(false);
        if(type == 0) 
        { 
            PoorSkins[UnityEngine.Random.Range(0, PoorSkins.Length)].SetActive(true);
            BuyObjCount = UnityEngine.Random.Range(1, 4);
        }
        else if (type == 1) 
        { 
            RichSkins[UnityEngine.Random.Range(0, RichSkins.Length)].SetActive(true);
            BuyObjCount = UnityEngine.Random.Range(4, 10);
        }
        else 
        { 
            BloggerSkins[UnityEngine.Random.Range(0, BloggerSkins.Length)].SetActive(true);
            BuyObjCount = UnityEngine.Random.Range(10, 16);
        }
    }
    public void ChangeBuyCount()
    {
        BuyObjCount--;
        OrderCount.text = BuyObjCount.ToString();
        if (BuyObjCount == 0)
        {
            OrderBar.SetActive(false);

        }
    }

    public void GoAway()
    {
        if (kiosk)
        {
            target = "Delete";
            targetPos = DeletePlace.transform;
            st.QueueList.Remove(gameObject);
            st.Queue = st.QueueList.ToArray();
        }
        else
        {

            if (!play)
            {
                int a = 0;
                for (int i = 0; i < meneger.ArcadeMachines.Length; i++)
                {
                    if (meneger.ArcadeMachines[i].gameObject.activeSelf)
                    {
                        if (meneger.ArcadeMachines[i].GetComponent<ArcadeMachine>().CanWork && !meneger.ArcadeMachines[i].GetComponent<ArcadeMachine>().hasTarget)
                        {
                            a++;
                        }
                    }
                }
                if (a > 0)
                {
                    for (int i = 0; i < meneger.ArcadeMachines.Length; i++)
                    {
                        if (meneger.ArcadeMachines[i].GetComponent<ArcadeMachine>().CanWork && !meneger.ArcadeMachines[i].GetComponent<ArcadeMachine>().hasTarget)
                        {
                            target = "Play";
                            meneger.ArcadeMachines[i].GetComponent<ArcadeMachine>().hasTarget = true;
                            machine = meneger.ArcadeMachines[i].GetComponent<ArcadeMachine>();
                            targetPos = machine.CustomerPos.transform;
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
            else if (play && !toilet)
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
                            //st.QueueList.Remove(gameObject);
                            //st.Queue = st.QueueList.ToArray();
                            toilet = true;
                            break;
                        }
                    }
                }
                else
                {
                    target = "Delete";
                    targetPos = DeletePlace.transform;
                    //st.QueueList.Remove(gameObject);
                    //st.Queue = st.QueueList.ToArray();
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
    }

    public int GetPosinQueue()
    {
        int a = 0;

        if (CanChangePos)
        {

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
        else
        {
            return ppos;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerTakeDropSystem>() != null)
        {
            if (isCalling)
            {
                StartCoroutine(Calling());
            }
        }
    }

    public void MakeCall()
    {
        CallTime = 200;
        step = CallTime;
        CallBar.fillAmount = 1;
        CallIcon.SetActive(true);
        agent.speed = 0;
        OrderBar.SetActive(false);
        isCalling = true;
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
            if (GetPosinQueue() == 0)
            {
                OrderBar.SetActive(true);
            }
            CallIcon.SetActive(false);
            isCalling = false;
            st.isCalling = false;
            for (int i = 0; i < st.Queue.Length; i++)
            {
                st.Queue[i].GetComponent<Customer>().agent.speed = 5;
            }
            if (st.LustCustomer != null)
            {
                st.LustCustomer = null;
            }
            st.LustCustomerPosinQueue = -1;
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
