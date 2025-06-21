using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateStation : MonoBehaviour
{
    public StationObjectCollection station;
    public GameObject[] RefObj;
    public GameObject MaxTxt;
    public float TimeToCreate, TimeCreateCar;

    private GameObject CarNow;
    private int CountUploads;
    public Transform MenegerPlace;

    public int lvl;
    public BuySpace space;
    public int[] PriceList;
    public bool CanWork;

    void Start()
    {
        StartCoroutine(CarCreat());
    }

    private void Update()
    {
        CanWork = gameObject.activeSelf;
        if (station.CountObj < station.MuxCountObj && MaxTxt.activeSelf)
        {
            MaxTxt.SetActive(false);
        }
    }

    private IEnumerator CarCreat()
    {
        yield return new WaitForSeconds(TimeCreateCar);
        //CarNow = Instantiate(Car, CarPlace.transform.position, Car.transform.rotation);
        StartCoroutine(Creating());
        CountUploads = station.MuxCountObj - station.CountObj;
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
            Vector3 pos;
            int a = Random.Range(0, RefObj.Length);
            if (station.CountObj == 0) { pos = station.PlaceObj.transform.position; }
            else { pos = station.ObjectsList[station.ObjectsList.Count - 1].GetComponent<Object>().UpPos.position - RefObj[a].GetComponent<Object>().DownPos.localPosition; }
            GameObject obj = Instantiate(RefObj[a], pos, station.PlaceObj.transform.rotation);
            station.ObjectsList.Add(obj);
            station.CountObj++;
            CountUploads--;
            if(station.CountObj == station.MuxCountObj)
            {
                MaxTxt.SetActive(true);
            }
            StartCoroutine(Creating());
        }
        else
        {
            //CarNow.GetComponent<GenerateObjCar>().TurnAway();
            //  CarNow = null;
            StartCoroutine(CarCreat());
        }
    }

    public void BuyUpgrade()
    {
        GetComponent<Animator>().SetTrigger("Buy");
        if (lvl <= PriceList.Length)
        {
            gameObject.GetComponent<StationObjectCollection>().MuxCountObj += 5;
        }
        if (lvl < PriceList.Length)
        {
            space.ChangePrice(PriceList[lvl]);
        }
        if (lvl < 3) { lvl++; }
    }

}
