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
    public float CallTime, step;
    private Rigidbody rb;
    public NavMeshAgent agent;
    private string[] BuyType = { "Buy", "Play" };
    public string target;
    public CustomersMeneger meneger;
    public int BuyObjCount;
    public SellStation st;
    public ArcadeMachine machine;

    public bool play, toilet, kiosk;
    public Animator _animator;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        OrderBar.SetActive(false);
        if (target == "Buy")
        {
            targetPos = st.CustomerPlace[GetPosinQueue()];
            OrderCount.text = BuyObjCount.ToString();
        }

        _animator.SetBool("IsMoving", true);
    }


    void Update()
    {
        if (OrderBar != null) { OrderBar.transform.LookAt(Camera.main.transform); }
        if (CallBar.activeSelf) { CallBar.transform.LookAt(Camera.main.transform); }
        agent.SetDestination(targetPos.position);



        if (target == "Buy")
        {
            targetPos = st.CustomerPlace[GetPosinQueue()];

            if (transform.position.x == targetPos.position.x && transform.position.z == targetPos.position.z)
            {

                if (st.isCalling)
                {
                    if (GetPosinQueue() > st.CallingCustomer.GetPosinQueue())
                    {
                        agent.speed = 0;
                        StartCoroutine(animator1("IsMoving", false, 0));
                        StartCoroutine(animator1("IsTalking", false, 0));
                    }
                }

                if (targetPos == st.CustomerPlace[0])
                {
                    if (!OrderBar.activeSelf & BuyObjCount > 0) { OrderBar.SetActive(true); }
                    st.client = this;
                    st.CountObj = BuyObjCount;

                }
                //остановка анимации, при вступлении в очередь
                foreach (var item in st.CustomerPlace)
                {
                    if (targetPos == item)
                        StartCoroutine(animator1("IsMoving", false, 0));
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
            if(_animator.GetBool("IsPlay") != true) 
                StartCoroutine(animator1("IsMoving", true, 0));
            if (transform.position.x == targetPos.position.x && transform.position.z == targetPos.position.z)
            {
                machine.client = this;
                if (!machine.isPlaying && machine.CanWork)
                {
                    //transform.Rotate(st.transform.position);
                    //transform.LookAt(st.transform.position);
                    Vector3 dir = machine.transform.position - this.transform.position;
                    this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.Euler(dir),1f);
                    StartCoroutine(animator1("IsPlay", true, 0));
                    StartCoroutine(animator1("IsMoving", false, 0));
                    machine.StartPlay();
                    StartCoroutine(animator1("IsPlay", false, 5));


                }
            }
        }
        if (transform.position.x == DeletePlace.position.x && transform.position.z == DeletePlace.position.z) { Destroy(gameObject); }
    }
    private IEnumerator animator1(string parametr, bool tf, int sec)
    {
        yield return new WaitForSeconds(sec);
        _animator.SetBool(parametr, tf);
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
        _animator.SetBool("IsMoving", true);
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
        CallBar.GetComponent<Image>().fillAmount = 1;
        agent.speed = 0;
        OrderBar.SetActive(false);
        CallBar.SetActive(true);
        isCalling = true;
        _animator.SetBool("IsTalking", true);
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
            CallBar.SetActive(false);
            isCalling = false;
            _animator.SetBool("IsTalking", false);
            _animator.SetBool("IsMoving", true);
            st.isCalling = false;
            for (int i = 0; i < st.Queue.Length; i++)
            {
                st.Queue[i].GetComponent<Customer>().agent.speed = 5;
            }
            if (st.LustCustomer != null)
            {
                st.LustCustomer = null;
            }
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
