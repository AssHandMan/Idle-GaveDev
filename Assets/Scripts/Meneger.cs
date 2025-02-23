using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Meneger : MonoBehaviour
{
    
    public float speed, capacity, profit;
    private NavMeshAgent agent;

    public string TargetType;
    public Transform Target;
    public MenegerHands hands;
    
    public ArcadeMachine[] arcade;
    public Trashbin bin;
    public SellStation[] Cashmachines;
    public CreateStation[] CreateStations;
    public CarSellStation CarsStations;
    public WorkStation workStation;
    public SellStation kiosk;
    public HrRoom room;

    public SellStation Cashmachine;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        
    }

    
    void Update()
    {
        if(agent.speed != speed) { agent.speed = speed; }
        
        if (TargetType == "")
        {
            DefineTarget();
        }

        if (TargetType == "Arcade")
        {
            agent.SetDestination(Target.position);
            if(hands.Count > 0)
            {
                TargetType = "Delete";
                Target = bin.MenegerPlace;
            }
        }
        else if (TargetType == "Delete")
        {
            agent.SetDestination(Target.position);
            if (hands.Count == 0)
            {
                TargetType = "";
                Target = null;
            }
        }
        else if(TargetType == "CreateStation")
        {
            if(Target == null && Cashmachine == Cashmachines[1]) {Target = GetCreateStation(Cashmachines[1]).MenegerPlace; }

            agent.SetDestination(Target.position);
            if ((hands.Count == capacity && capacity <= Cashmachine.station.MuxCountObj) || (hands.Count == Cashmachine.station.MuxCountObj && capacity >= Cashmachine.station.MuxCountObj))
            {
                TargetType = "AddToCashmachine";
            }
        }
        else if(TargetType == "AddToCashmachine")
        {
            agent.SetDestination(Cashmachine.MenegerPlace.position);
            if (hands.Count == 0)
            {
                int CountArcade = 0;
                for (int i = 0; i < arcade.Length; i++ )
                {
                    if (!arcade[i].CanWork & arcade[i].station.CountObj > 0 & !arcade[i].hasMenegerTarget)
                    {
                        CountArcade++;
                    }
                }
                if(CountArcade > 0) 
                {
                    //TargetType = null; Target = null;;
                    DefineTarget();
                }
                else if(!Cashmachine.isSelling)
                {
                    room.CheckWork();
                    if (!room.isWorkingCash)
                    {
                        Target = Cashmachine.WorkSpace.transform;
                        TargetType = "WorkerCashmachine";
                    }
                }
                else
                {
                    DefineTarget();
                }
            }
        }
        else if (TargetType == "WorkerCashmachine")
        {
            agent.SetDestination(Target.position);

            int CountArcade = 0;
            for (int i = 0; i < arcade.Length; i++)
            {
                if (!arcade[i].CanWork & arcade[i].station.CountObj > 0)
                {
                    CountArcade++;
                }
            }

            if (Cashmachine.station.CountObj == 0 || CountArcade > 0)
            {
                
                DefineTarget();
            }
        }
    }

    public CreateStation GetCreateStation(SellStation CashMachine)
    {
        if (CashMachine == Cashmachines[0])
        {
            if (CreateStations[0].station.CountObj >= CreateStations[1].station.CountObj) { return CreateStations[0]; }
            else { return CreateStations[1]; }
        }
        else
        {
            if (CreateStations[2].station.CountObj >= CreateStations[3].station.CountObj) { return CreateStations[2]; }
            else { return CreateStations[3]; }
        }
    }

    public void DefineTarget()
    {
        int CountArcade = 0;
        for(int i = 0; i < arcade.Length; i++)
        {
            if (!arcade[i].CanWork & arcade[i].station.CountObj > 0 & !arcade[i].hasMenegerTarget)
            {
                CountArcade++;
            }
        }
        if(CountArcade > 0)
        {
            for (int i = 0; i < arcade.Length; i++)
            {
                if (!arcade[i].CanWork & arcade[i].station.CountObj > 0 & !arcade[i].hasMenegerTarget)
                {
                    Target = arcade[i].CustomerPos;
                    arcade[i].hasMenegerTarget = true;
                    break;
                }
            }
            TargetType = "Arcade";
        }
        else
        {
            int type = -1;
            while (type != 0 && (Cashmachines[0].CanWork || Cashmachines[1].CanWork)) 
            { 
                type = UnityEngine.Random.Range(0, 3);

            }
            
            if(type == 0)
            {
                if (Cashmachines[0].CanWork)
                {
                    if (Cashmachines[0].station.CountObj == 0)
                    {
                        Target = GetCreateStation(Cashmachines[0]).MenegerPlace;
                        TargetType = "CreateStation";
                        Cashmachine = Cashmachines[0];
                    }
                    else
                    {
                        room.CheckWork();
                        if (!Cashmachine.isSelling && !room.isWorkingCash)
                        {
                            Target = Cashmachine.WorkSpace.transform;
                            TargetType = "WorkerCashmachine";
                            Cashmachine = Cashmachines[0];
                        }
                    }
                }
                if(Cashmachines[1].CanWork & Cashmachine == null)
                {
                    if (Cashmachines[1].station.CountObj == 0)
                    {
                        Target = GetCreateStation(Cashmachines[1]).MenegerPlace;
                        TargetType = "CreateStation";
                        Cashmachine = Cashmachines[1];
                    }
                    else
                    {
                        room.CheckWork();
                        if (!Cashmachine.isSelling && !room.isWorkingCash)
                        {
                            Target = Cashmachine.WorkSpace.transform;
                            TargetType = "WorkerCashmachine";
                            Cashmachine = Cashmachines[1];
                        }
                    }
                }    
            }
        }
    }

    private IEnumerator DefiningBreak()
    {
        yield return new WaitForSeconds(2);
        DefineTarget();
    }
}
