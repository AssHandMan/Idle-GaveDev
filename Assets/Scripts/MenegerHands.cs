using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenegerHands : MonoBehaviour
{
    public Meneger meneger;
    public List<GameObject> IngredientsCollection;
    public int Count;
    public float TakeSpeed;
    public CreateStation createStation;
    public DropStation dropStation;
    public TakeStation takeStation;

    public bool isTakingFromTakeStation;
    private int CountForLustObj;
    private GameObject LustTakenObj, PrevTakenObj;
    public bool inProgress;
    private Vector3 startPos;
    private Vector3 endPos;
    private Vector3 controlPoint;
    private float arcT = 0f;

    void Start()
    {
        
    }

    void Update()
    {
        if (inProgress)
        {
            if (LustTakenObj.transform.parent == GameObject.FindGameObjectWithTag("Player").transform && LustTakenObj.GetComponent<Object>().Type == "Trash")
            {
                inProgress = false;
            }
            
            arcT += Time.deltaTime * TakeSpeed;
            arcT = Mathf.Clamp01(arcT);

            if (CountForLustObj == 0) { endPos = transform.position; }
            else { endPos = PrevTakenObj.GetComponent<Object>().UpPos.position - LustTakenObj.GetComponent<Object>().DownPos.localPosition; }

            controlPoint = (startPos + endPos) / 2f;
            controlPoint.y += 3f;

            Vector3 pos = Mathf.Pow(1 - arcT, 2) * startPos + 2 * (1 - arcT) * arcT * controlPoint + Mathf.Pow(arcT, 2) * endPos;
            LustTakenObj.transform.position = pos;

            if (arcT >= 1f)
            {
                LustTakenObj.transform.position = endPos;
                LustTakenObj.transform.parent = transform;
                LustTakenObj.transform.rotation = transform.rotation;
                IngredientsCollection.Add(LustTakenObj);
                Count++;
                LustTakenObj.GetComponent<Object>().ChangeTrail();
                LustTakenObj = null;
                inProgress = false;
            }
        }
        if(Count == 0 && transform.childCount > 0)
        {
            Destroy(transform.GetChild(0).gameObject);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if(meneger.TargetType == "Arcade" && other.GetComponent<ArcadeMachine>() != null)
        {
            ArcadeMachine arcade = other.GetComponent<ArcadeMachine>();
            if(meneger.Target == arcade.CustomerPos)
            {
                if (arcade.station.CountObj != 0)
                {
                    GameObject newObj = arcade.station.ObjectsList[0];
                    arcade.GetComponent<TakeStation>().RemoveObj();

                    CountForLustObj = arcade.station.CountObj;
                    GameObject prevObj = (Count > 0) ? IngredientsCollection[IngredientsCollection.Count - 1] : null;
                    PrevTakenObj = prevObj;
                    LustTakenObj = newObj;
                    startPos = newObj.transform.position;
                    arcT = 0f;  
                    inProgress = true;
                    arcade.hasMenegerTarget = false;
                }
                else
                {
                    if(!inProgress && Count == 0)
                    {
                        meneger.DefineTarget();
                    }
                }
            }
        }
        else if (meneger.TargetType == "Delete" && other.GetComponent<Trashbin>() != null)
        {
            if (meneger.Target == other.GetComponent<Trashbin>().MenegerPlace)
            {
               other.GetComponent<Trashbin>().AddObj(IngredientsCollection[Count - 1], TakeSpeed);
               IngredientsCollection.RemoveAt(Count - 1);
               Count--;
            }
        }
        else if ((meneger.TargetType == "CreateStation" || meneger.TargetType == "CreateStation2" || meneger.TargetType == "CreateStationCar" || meneger.TargetType == "CreateKiosk1" || meneger.TargetType == "CreateKiosk2") && other.GetComponent<CreateStation>() != null)
        {
            if (meneger.Target == other.GetComponent<CreateStation>().MenegerPlace)
            {
                createStation = other.GetComponent<CreateStation>();
                StartCoroutine(GetFromCreateStation());
            }
        }
        else if (meneger.TargetType == "AddToCashmachine" && other.GetComponent<DropStation>() != null)
        {
            dropStation = other.GetComponent<DropStation>();
            StartCoroutine(AddToCashMachine());
        }
        else if ((meneger.TargetType == "AddToWorkStation" || meneger.TargetType == "AddToCashMachineCar" || meneger.TargetType == "AddToKiosk1" || meneger.TargetType == "AddToKiosk2") && other.GetComponent<DropStation>() != null)
        {
            dropStation = other.GetComponent<DropStation>();
            StartCoroutine(AddToCashMachine());
        }
        else if(meneger.TargetType == "TakeStationCar" && other.GetComponent<TakeStation>() != null)
        {
            takeStation = other.GetComponent<TakeStation>();
            StartCoroutine(GetFromTakeStation());
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if (meneger.TargetType == "TakeStationCar" && other.GetComponent<TakeStation>() != null && other.GetComponent<CreateStation>() == null)
        {
            if(!isTakingFromTakeStation)
            {
                takeStation = other.GetComponent<TakeStation>();
                StartCoroutine(GetFromTakeStation());
                isTakingFromTakeStation = true;
            }
        }
    }

    private IEnumerator GetFromCreateStation()
    {
        yield return new WaitForSeconds(0.2f);
        if (meneger.TargetType == "CreateStation" || meneger.TargetType == "CreateStation2" || meneger.TargetType == "CreateKiosk1" || meneger.TargetType == "CreateKiosk2")
        {
            if (createStation.station.CountObj == 0 && Count <= 0) { StartCoroutine(WaitNewCreateObj()); }
            else if (Count < meneger.capacity)
            {
                GameObject newObj = createStation.station.ObjectsList[createStation.station.ObjectsList.Count - 1];
                createStation.gameObject.GetComponent<TakeStation>().RemoveObj();

                CountForLustObj = Count;
                GameObject prevObj = (Count > 0) ? IngredientsCollection[IngredientsCollection.Count - 1] : null;
                PrevTakenObj = prevObj;
                LustTakenObj = newObj;
                startPos = newObj.transform.position;
                arcT = 0f;

                inProgress = true;

                while(inProgress)
                {
                    yield return null;
                }
                if (Count < meneger.capacity && createStation.station.CountObj > 0) { StartCoroutine(GetFromCreateStation()); }
            }
        }
        else if(meneger.TargetType == "CreateStationCar")
        {
            if (createStation.station.CountObj == 0 && Count <= 0) { StartCoroutine(WaitNewCreateObj()); }
            else if (Count < meneger.capacity)
            {
                GameObject newObj = createStation.station.ObjectsList[createStation.station.ObjectsList.Count - 1];
                createStation.gameObject.GetComponent<TakeStation>().RemoveObj();

                CountForLustObj = Count;
                GameObject prevObj = (Count > 0) ? IngredientsCollection[IngredientsCollection.Count - 1] : null;
                PrevTakenObj = prevObj;
                LustTakenObj = newObj;
                startPos = newObj.transform.position;
                arcT = 0f;

                inProgress = true;

                while (inProgress)
                {
                    yield return null;
                }

                if (Count < meneger.capacity && createStation.station.CountObj > 0) { StartCoroutine(GetFromCreateStation()); }
            }
        }
    }

    private IEnumerator WaitNewCreateObj()
    {
        yield return new WaitForSeconds(0.2f);
        StartCoroutine(GetFromCreateStation());
    }

    private IEnumerator WaitNewCreateObjÑar()
    {
        yield return new WaitForSeconds(0.2f);
        StartCoroutine(GetFromTakeStation());
    }

    private IEnumerator AddToCashMachine()
    {
        if(meneger.TargetType == "AddToKiosk1")
        {
            yield return new WaitForSeconds(0.2f);
            if (Count > 0)
            {
                IngredientsCollection[Count - 1].GetComponent<Object>().ChangeTrail();
                while (meneger.KioskDrop1.inProgress)
                {
                    yield return null;
                }
                meneger.KioskDrop1.AddObj(IngredientsCollection[Count - 1], TakeSpeed);
                IngredientsCollection.RemoveAt(Count - 1);
                Count--;
                if (Count > 0) { StartCoroutine(AddToCashMachine()); }
            }
        }
        else if (meneger.TargetType == "AddToKiosk2")
        {
            yield return new WaitForSeconds(0.2f);
            if (Count > 0)
            {
                IngredientsCollection[Count - 1].GetComponent<Object>().ChangeTrail();
                while (meneger.KioskDrop2.inProgress)
                {
                    yield return null;
                }
                meneger.KioskDrop2.AddObj(IngredientsCollection[Count - 1], TakeSpeed);
                IngredientsCollection.RemoveAt(Count - 1);
                Count--;
                if (Count > 0) { StartCoroutine(AddToCashMachine()); }
            }
        }
        else
        {
            yield return new WaitForSeconds(0.2f);
            if (Count > 0)
            {
                IngredientsCollection[Count - 1].GetComponent<Object>().ChangeTrail();
                while (dropStation.inProgress)
                {
                    yield return null;
                }
                dropStation.AddObj(IngredientsCollection[Count - 1], TakeSpeed);
                IngredientsCollection.RemoveAt(Count - 1);
                while (dropStation.inProgress)
                {
                    yield return null;
                }
                Count--;
                if (Count > 0) { StartCoroutine(AddToCashMachine()); }
            }
        }
    }

    private IEnumerator GetFromTakeStation()
    {
        yield return new WaitForSeconds(0.2f);
        if (meneger.workStation.newStation.CountObj == 0 && meneger.workStation.station.CountObj > 0 && Count < meneger.capacity) { StartCoroutine(WaitNewCreateObjÑar()); }
        else if (meneger.workStation.newStation.CountObj > 0)
        {
            GameObject newObj = takeStation.station.ObjectsList[takeStation.station.ObjectsList.Count - 1];
            takeStation.gameObject.GetComponent<TakeStation>().RemoveObj();

            CountForLustObj = Count;
            GameObject prevObj = (Count > 0) ? IngredientsCollection[IngredientsCollection.Count - 1] : null;
            PrevTakenObj = prevObj;
            LustTakenObj = newObj;
            startPos = newObj.transform.position;
            arcT = 0f;

            inProgress = true;

            while (inProgress)
            {
                yield return null;
            }

            if (meneger.workStation.newStation.CountObj > 0) { StartCoroutine(GetFromTakeStation()); }
            else if (meneger.workStation.newStation.CountObj == 0 && meneger.workStation.station.CountObj > 0 && Count < meneger.capacity) { StartCoroutine(WaitNewCreateObjÑar()); }
            else { isTakingFromTakeStation = false; }
        }
    }

}
