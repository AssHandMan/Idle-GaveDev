using System;
using System.Collections;
using System.Collections.Generic;
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
    public Trashbin[] bin;
    public SellStation[] Cashmachines;
    public CreateStation[] CreateStations;
    public CarSellStation CarsStations;
    public WorkStation workStation;
    public Kiosk kiosk;
    public DropStation KioskDrop1, KioskDrop2;
    public HrRoom room;

    public SellStation Cashmachine;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if(capacity != capacityUpgreadeBufer) { capacity = capacityUpgreadeBufer; }
    }

    
    void Update()
    {
        if (agent.speed != speed) { agent.speed = speed; }
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
                if (Cashmachines[1].CanWork)
                {
                    if (transform.localPosition.x < 17) { Target = bin[0].MenegerPlace; }
                    else { Target = bin[1].MenegerPlace; }
                }
                else
                {
                    Target = bin[0].MenegerPlace;
                }
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
        if(TargetType == "CreateStation" || TargetType == "CreateStation2" || TargetType == "CreateStationCar" || TargetType == "CreateKiosk1" || TargetType == "CreateKiosk2")
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
                if ((hands.Count == capacity && capacity <= workStation.newStation.MuxCountObj) || (hands.Count == workStation.newStation.MuxCountObj && capacity >= workStation.newStation.MuxCountObj))
                {
                    TargetType = "AddToWorkStation";
                    Target = workStation.SaveStationPlace;
                    room.AddTarget("AddToWorkStation");
                }
            }
            else if (TargetType == "CreateKiosk1" || TargetType == "CreateKiosk2")
            {
                if (hands.Count == capacity)
                {
                    if(TargetType == "CreateKiosk1") { TargetType = "AddToKiosk1"; }
                    else if (TargetType == "CreateKiosk2") { TargetType = "AddToKiosk2"; }
                    Target = kiosk.menegerPlace;
                }
            }
        }
        if(TargetType == "AddToKiosk1" || TargetType == "AddToKiosk2")
        {
            agent.SetDestination(Target.position);
            if(hands.Count == 0)
            {
                if (TargetType == "AddToKiosk1") { room.RemoveTarget("CreateKiosk1"); }
                else if (TargetType == "AddToKiosk2") { room.RemoveTarget("CreateKiosk2"); }
                DefineTarget();
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
            if (hands.Count == 0 && workStation.lvl < 2)
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
            if (hands.Count == 0 && !hands.inProgress)
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
                TargetType = "";
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
            if (workStation.CanWork && CarsStations.CanWork) { TargetTypes.Add(1); }
            if (kiosk.GetComponent<SellStation>().CanWork && CreateStations[2].CanWork) { TargetTypes.Add(2); }
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
                    if (workStation.station.CountObj == 0 && TargetType != "Corotune")
                    {
                        TargetType = "Corotune";
                        StartCoroutine(TimeToGetCarCreate());
                    }
                }

            }
            if(type == 2)
            {
                if(!room.CheckWork("CreateKiosk1") && kiosk.station1.CountObj == 0)
                {
                    TargetType = "CreateKiosk1";
                    room.AddTarget("CreateKiosk1");
                    Target = GetCreateStation(Cashmachines[1]).MenegerPlace;
                }
                else if (!room.CheckWork("CreateKiosk2") && kiosk.station2.CountObj == 0)
                {
                    TargetType = "CreateKiosk2";
                    room.AddTarget("CreateKiosk2");
                    Target = GetCreateStation(Cashmachines[0]).MenegerPlace;
                }
            }
        }
    }

    private IEnumerator TimeToGetCarCreate()
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(0.1f, 1f));
        if (!room.CheckWork("CreateStationCar") && !room.CheckWork("WorkWork") && !room.CheckWork("TakeStationCar") && !room.CheckWork("CashCarWork") && !room.CheckWork("AddToCashMachineCar"))
        {
            if (workStation.station.CountObj == 0 && !room.CheckWork("CreateStationCar"))
            {
                room.AddTarget("CreateStationCar");
                Target = GetCreateStation(Cashmachines[0]).MenegerPlace;
                TargetType = "CreateStationCar";
            }
        }
        if(TargetType == "Corotune")
        {
            TargetType = "";
        }
    }
}
