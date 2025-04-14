using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenegerHands : MonoBehaviour
{
    public Meneger meneger;
    public List<GameObject> IngredientsCollection;
    public int Count;

    private CreateStation createStation;
    public DropStation dropStation;
    public TakeStation takeStation;

    public bool isTakingFromTakeStation;
    void Start()
    {
        
    }

    void Update()
    {
        
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
                    Vector3 pos;
                    if (Count == 0) { pos = transform.position; }
                    else { pos = IngredientsCollection[IngredientsCollection.Count - 1].GetComponent<Object>().UpPos.position - arcade.station.ObjectsList[arcade.station.CountObj - 1].GetComponent<Object>().DownPos.localPosition; }
                    GameObject newObj = Instantiate(arcade.station.ObjectsList[arcade.station.CountObj - 1], pos, transform.rotation);
                    newObj.transform.parent = transform;
                    IngredientsCollection.Add(newObj);
                    Count++;
                    arcade.GetComponent<TakeStation>().DeleteObj();
                    arcade.hasMenegerTarget = false;
                }
                else
                {
                    arcade.hasMenegerTarget = false;
                    meneger.DefineTarget();
                }
            }
        }
        else if (meneger.TargetType == "Delete" && other.GetComponent<Trashbin>() != null)
        {
            if (meneger.Target == other.GetComponent<Trashbin>().MenegerPlace)
            {
                Destroy(IngredientsCollection[Count - 1]);
                IngredientsCollection.RemoveAt(Count - 1);
                Count--;
            }
        }
        else if ((meneger.TargetType == "CreateStation" || meneger.TargetType == "CreateStation2" || meneger.TargetType == "CreateStationCar" || meneger.TargetType == "CreateKiosk1" || meneger.TargetType == "CreateKiosk2") && other.GetComponent<CreateStation>() != null)
        {
            Debug.Log("1");
            if (meneger.Target == other.GetComponent<CreateStation>().MenegerPlace)
            {
                createStation = other.GetComponent<CreateStation>();
                StartCoroutine(GetFromCreateStation());
                Debug.Log("2");
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
            if (createStation.station.CountObj == 0) { StartCoroutine(WaitNewCreateObj()); }
            else if (Count < meneger.capacity)
            {
                Vector3 pos;
                if (Count == 0) { pos = transform.position; }
                else { pos = IngredientsCollection[IngredientsCollection.Count - 1].GetComponent<Object>().UpPos.position - createStation.station.ObjectsList[createStation.station.CountObj - 1].GetComponent<Object>().DownPos.localPosition;  }
                GameObject newObj = Instantiate(createStation.station.ObjectsList[createStation.station.CountObj - 1], pos, transform.rotation);
                newObj.transform.parent = transform;
                IngredientsCollection.Add(newObj);
                Count++;
                createStation.GetComponent<TakeStation>().DeleteObj();

                if(Count < meneger.capacity) { StartCoroutine(GetFromCreateStation()); }
            }
        }
        else if(meneger.TargetType == "CreateStationCar")
        {
            Debug.Log("3");
            if (createStation.station.CountObj == 0) { StartCoroutine(WaitNewCreateObj()); }
            else if (Count < meneger.capacity)
            {
                Vector3 pos;
                if (Count == 0) { pos = transform.position; }
                else { pos = IngredientsCollection[IngredientsCollection.Count - 1].GetComponent<Object>().UpPos.position - createStation.station.ObjectsList[createStation.station.CountObj - 1].GetComponent<Object>().DownPos.localPosition; }
                GameObject newObj = Instantiate(createStation.station.ObjectsList[createStation.station.CountObj - 1], pos, transform.rotation);
                newObj.transform.parent = transform;
                IngredientsCollection.Add(newObj);
                Count++;
                createStation.GetComponent<TakeStation>().DeleteObj();

                if (Count < meneger.capacity) { StartCoroutine(GetFromCreateStation()); }
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
                meneger.KioskDrop1.AddObj(IngredientsCollection[Count - 1]);

                Destroy(IngredientsCollection[Count - 1]);
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
                meneger.KioskDrop2.AddObj(IngredientsCollection[Count - 1]);

                Destroy(IngredientsCollection[Count - 1]);
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
                dropStation.AddObj(IngredientsCollection[Count - 1]);

                Destroy(IngredientsCollection[Count - 1]);
                IngredientsCollection.RemoveAt(Count - 1);
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
            Vector3 pos;
            if (Count == 0) { pos = transform.position; }
            else { pos = IngredientsCollection[IngredientsCollection.Count - 1].GetComponent<Object>().UpPos.position - takeStation.station.ObjectsList[takeStation.station.CountObj - 1].GetComponent<Object>().DownPos.localPosition; }
            GameObject newObj = Instantiate(takeStation.station.ObjectsList[takeStation.station.CountObj - 1], pos, transform.rotation);
            newObj.transform.parent = transform;
            IngredientsCollection.Add(newObj);
            Count++;
            takeStation.GetComponent<TakeStation>().DeleteObj();

            if (meneger.workStation.newStation.CountObj > 0) { StartCoroutine(GetFromTakeStation()); }
            else { isTakingFromTakeStation = false; }
        }
        Debug.Log("WHAT");
    }
}
