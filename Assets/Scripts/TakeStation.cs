using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeStation : MonoBehaviour
{
    public StationObjectCollection station;
    public bool CanGive;

    void Start()
    {

    }


    void Update()
    {
        if (station.CountObj > 0) { CanGive = true; }
        else { CanGive = false; }
    }

    public void DeleteObj()
    {
        Destroy(station.ObjectsList[station.CountObj - 1]);
        station.ObjectsList.RemoveAt(station.CountObj - 1);
        station.CountObj--;
    }
    public void RemoveObj()
    {
        station.ObjectsList.RemoveAt(station.CountObj - 1);
        station.CountObj--;
    }

}
