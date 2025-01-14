using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateStation : MonoBehaviour
{
    public StationObjectCollection station;
    public GameObject RefObj, Car, CarPlace;
    public float TimeToCreate, TimeCreateCar;

    private GameObject CarNow;
    private int CountUploads;

    void Start()
    {
        station.LustPosY = station.PlaceObj.transform.position.y;
        StartCoroutine(CarCreat());
    }

    private IEnumerator CarCreat()
    {
        yield return new WaitForSeconds(TimeCreateCar);
        CarNow = Instantiate(Car, CarPlace.transform.position, Car.transform.rotation);
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<GenerateObjCar>() != null && other.GetComponent<GenerateObjCar>().CanGive)
        {
            other.GetComponent<GenerateObjCar>().CanMove = false;
            CountUploads = station.MuxCountObj - station.CountObj;
            StartCoroutine(Creating());
        }
    }

    private IEnumerator Creating()
    {

        yield return new WaitForSeconds(TimeToCreate);

        if (station.CountObj < station.MuxCountObj && CountUploads != 0)
        {
            Vector3 pos = new Vector3(station.PlaceObj.transform.position.x, station.LustPosY, station.PlaceObj.transform.position.z);
            GameObject obj = Instantiate(RefObj, pos, RefObj.transform.rotation);
            station.ObjectsList.Add(obj);
            station.LustPosY += 0.1f;
            station.CountObj++;
            CountUploads--;
            StartCoroutine(Creating());
        }
        else
        {
            CarNow.GetComponent<GenerateObjCar>().TurnAway();
            CarNow = null;
            StartCoroutine(CarCreat());
        }
    }

    public void OnDestroy()
    {
        if(CarNow != null && !CarNow.GetComponent<GenerateObjCar>().CanMove) 
        {
            CarNow.GetComponent<GenerateObjCar>().isNewUdpate = true;
            CarNow = null;
        }
        StopAllCoroutines();
        station.OnDestroy();
    }
}
