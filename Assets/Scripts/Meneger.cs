using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class Meneger : MonoBehaviour
{
    public CustomersMeneger customerMeneger;
    public MenegerHands hands;
    public Trashbin trashbin;
    public CreateStation Create1, Create2;
    public float speed, capacity, profit;
    private NavMeshAgent agent;

    public bool inProgress;
    public Transform target;
    public Transform[] Targets;
    public List<Transform> pos = new List<Transform>();

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        StartCoroutine(DefineTarget());
    }

    
    void Update()
    {
        if(inProgress)
        {
            if (Targets.Length > 0)
            {
                target = Targets[0];
                agent.SetDestination(target.position);
                if (transform.position.x == Targets[0].position.x && transform.position.z == Targets[0].position.z)
                {
                    hands.gameObject.SetActive(true);
                }
            }
            else
            {
                target = trashbin.MenegerPlace;
                agent.SetDestination(target.position);
                if (transform.position.x == trashbin.MenegerPlace.position.x && transform.position.z == trashbin.MenegerPlace.position.z)
                {
                    hands.gameObject.SetActive(true);
                }
            }
        }
    }

    public void RemoveFirst()
    {
        pos.RemoveAt(0);
        Targets = pos.ToArray();
    }

    public IEnumerator DefineTarget()
    {
        int a = 0;
        for(int i = 0; i < customerMeneger.ArcadeMachines.Length; i++)
        {
            if(a != Convert.ToInt32(capacity))
            {
                if(customerMeneger.ArcadeMachines[i].GetComponent<ArcadeMachine>().station.CountObj > 0)
                {
                    pos.Add(customerMeneger.ArcadeMachines[i].GetComponent<ArcadeMachine>().CustomerPos);
                    a++;
                }
            }
        }
        if (a>0)
        {
            inProgress = true;
            Targets = pos.ToArray();
        }
        else
        {
            yield return new WaitForSeconds(1);
            StartCoroutine(DefineTarget());
        }
    }
}
