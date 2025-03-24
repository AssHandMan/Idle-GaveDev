using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Meneger : MonoBehaviour
{
    public int CashType;

    public float speed, capacity, capacityUpgreadeBufer, profit;
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
    private static float cooldownTaskTime = 0f;
    private static float cooldownDuration = 0.5f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if(capacity != capacityUpgreadeBufer) { capacity = capacityUpgreadeBufer; }
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
        if (TargetType == "Delete")
        {
            agent.SetDestination(Target.position);
            if (hands.Count == 0)
            {
                TargetType = "";
                Target = null;
            }
        }
        if(TargetType == "CreateStation" || TargetType == "CreateStation2" || TargetType == "CreateStationCar")
        {
            if(Target == null && Cashmachine == Cashmachines[1]) {Target = GetCreateStation(Cashmachines[1]).MenegerPlace; }

            agent.SetDestination(Target.position);

            if (TargetType == "CreateStation" || TargetType == "CreateStation2")
            {
                if ((hands.Count == capacity && capacity <= Cashmachine.station.MuxCountObj) || (hands.Count == Cashmachine.station.MuxCountObj && capacity >= Cashmachine.station.MuxCountObj))
                {
                    TargetType = "AddToCashmachine";
                }
            }
            else if(TargetType == "CreateStationCar")
            {
                if ((hands.Count == capacity && capacity <= CarsStations.station.MuxCountObj) || (hands.Count == CarsStations.station.MuxCountObj && capacity >= CarsStations.station.MuxCountObj))
                {
                    TargetType = "AddToWorkStation";
                    Target = workStation.SaveStationPlace;
                    room.AddTarget("AddToWorkStation");
                }
            }
        }
        if(TargetType == "AddToCashmachine")
        {
            agent.SetDestination(Cashmachine.MenegerPlace.position);
            if (hands.Count == 0)
            {
                if (Cashmachine == Cashmachines[0]) { room.RemoveTarget("CreateStation"); }
                if (Cashmachine == Cashmachines[1]) { room.RemoveTarget("CreateStation2"); }
                DefineTarget();
            }
        }
        if (TargetType == "WorkerCashmachine" || TargetType == "WorkerCashmachine2")
        {
            agent.SetDestination(Target.position);

            if(transform.position.x > 14) { Cashmachine = Cashmachines[1]; }
           
            //CheackArcade();
            if (Cashmachine.station.CountObj == 0)
            {
                room.RemoveTarget(TargetType);
                DefineTarget();
            }
        }

        if(TargetType == "AddToWorkStation")
        {
            agent.SetDestination(Target.position);
            if (hands.Count == 0 && workStation.lvl < 2 && !workStation.isSelling)
            {
                Target = workStation.WorkPlace;
                TargetType = "WorkWork";
                room.AddTarget("WorkWork");
                room.RemoveTarget("AddToWorkStation");
            }
            else
            {
                if(hands.Count == 0)
                {
                    Target = workStation.NewStationPlace;
                    TargetType = "TakeStationCar";
                    room.AddTarget("TakeStationCar");
                    room.RemoveTarget("AddToWorkStation");
                }
            }
        }
        if(TargetType == "WorkWork")
        {
            agent.SetDestination(Target.position);
            if (workStation.station.CountObj == 0)
            {
                Target = workStation.NewStationPlace;
                TargetType = "TakeStationCar";
                room.AddTarget("TakeStationCar");
                room.RemoveTarget("WorkWork");
            }
        }
        if (TargetType == "TakeStationCar")
        {
            agent.SetDestination(workStation.NewStationPlace.position);
            if ((workStation.newStation.CountObj == 0 && workStation.station.CountObj == 0) || hands.Count == capacity)
            {
                TargetType = "AddToCashMachineCar";
                hands.isTakingFromTakeStation = false;
                room.AddTarget("AddToCashMachineCar");
                room.RemoveTarget("TakeStationCar");
            }
        }
        if (TargetType == "AddToCashMachineCar")
        {
            agent.SetDestination(CarsStations.SaveStationPlace.position);
            if (hands.Count == 0)
            {
                if (CarsStations.lvl < 2 && !CarsStations.isSelling)
                {
                    TargetType = "CashCarWork";
                    Target = CarsStations.WorkPlace;
                    room.AddTarget("CashCarWork");
                    room.RemoveTarget("AddToCashMachineCar");
                }
                else
                {
                    room.RemoveTarget("CreateStationCar");
                    room.RemoveTarget("AddToCashMachineCar");
                    DefineTarget();
                }
            }
        }

        if (TargetType == "CashCarWork")
        {
            agent.SetDestination(Target.position);
            if(CarsStations.station.CountObj == 0)
            {
                room.RemoveTarget("CreateStationCar");
                room.RemoveTarget("CashCarWork");
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

    public void CheackArcade()
    {
        int CountArcade = 0;
        for (int i = 0; i < arcade.Length; i++)
        {
            if (!arcade[i].CanWork & arcade[i].station.CountObj > 0 & !arcade[i].hasMenegerTarget)
            {
                CountArcade++;
            }
        }
        if (CountArcade > 0)
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
    }

    public void DefineTarget()
    {
        if (capacity != capacityUpgreadeBufer) { capacity = capacityUpgreadeBufer; }

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
            List<int> TargetTypes = new List<int>();
            if(Cashmachines[0].CanWork || Cashmachines[1].CanWork) { TargetTypes.Add(0); }
            if (workStation.CanWork && CarsStations.CanWork && !room.CheckWork("CreateStationCar") && !room.CheckWork("WorkWork") && !room.CheckWork("TakeStationCar") && !room.CheckWork("CashCarWork") && !room.CheckWork("AddToCashMachineCar")) { TargetTypes.Add(1); }
            int type = TargetTypes[UnityEngine.Random.Range(0, TargetTypes.Count)];

            if (type == 0)
            {
                int Cash = -1;

                if (CreateStations[2].CanWork) { Cash = UnityEngine.Random.Range(0, 2); }
                else { Cash = UnityEngine.Random.Range(0, 1); }

                if (Cash == 0 && Cashmachines[0].CanWork)
                {
                    if (Cashmachines[0].station.CountObj == 0 && !room.CheckWork("CreateStation"))
                    {
                        Target = GetCreateStation(Cashmachines[0]).MenegerPlace;
                        TargetType = "CreateStation";           
                        Cashmachine = Cashmachines[0];
                        room.AddTarget("CreateStation");
                        CashType = 1;
                    }
                    else
                    {
                        Cashmachine = Cashmachines[0];
                        if (!Cashmachine.isSelling && !room.CheckWork("WorkerCashmachine"))
                        {
                            Target = Cashmachine.WorkSpace.transform;
                            TargetType = "WorkerCashmachine";
                            room.AddTarget("WorkerCashmachine");
                            CashType = 1;
                        }
                    }
                }
                else if(Cash == 1 && Cashmachines[1].CanWork)
                {
                    if (Cashmachines[1].station.CountObj == 0 && !room.CheckWork("CreateStation2"))
                    {
                        Target = GetCreateStation(Cashmachines[1]).MenegerPlace;
                        TargetType = "CreateStation2";
                        Cashmachine = Cashmachines[1];
                        room.AddTarget("CreateStation2");
                        CashType = 2;
                    }
                    else
                    {
                        Cashmachine = Cashmachines[1];
                        if (!Cashmachines[1].isSelling && !room.CheckWork("WorkerCashmachine2"))
                        {
                            Target = Cashmachines[1].WorkSpace.transform;
                            TargetType = "WorkerCashmachine2";
                            room.AddTarget("WorkerCashmachine2");
                            CashType = 2;
                        }
                    }
                }    
            }
            if (type == 1)
            {
                /*if (CarsStations.station.CountObj > 0 && !room.CheckWork("CashCarWork"))
                {
                    TargetType = "CashCarWork";
                }
                else if (workStation.newStation.CountObj > 0 && !room.CheckWork("TakeStationCar"))
                {
                    Target = workStation.NewStationPlace;
                    TargetType = "TakeStationCar";
                }
                else if (workStation.station.CountObj > 0 && workStation.lvl < 2 && !workStation.isSelling && !room.CheckWork("WorkWork"))
                {
                    Target = workStation.WorkPlace;
                    TargetType = "WorkWork";
                }
                else*/ 
                if (!room.CheckWork("CreateStationCar") && !room.CheckWork("WorkWork") && !room.CheckWork("TakeStationCar") && !room.CheckWork("CashCarWork") && !room.CheckWork("AddToCashMachineCar"))
                {
                    if (workStation.station.CountObj == 0 && !room.CheckWork("CreateStationCar"))
                    {
                        room.AddTarget("CreateStationCar");
                        Target = GetCreateStation(Cashmachines[0]).MenegerPlace;
                        TargetType = "CreateStationCar";
                    }
                }

            }
        }
    }
}
