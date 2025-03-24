using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkStation : MonoBehaviour
{
    public StationObjectCollection station, newStation;
    public TakeStation TakeStationNew;
    public GameObject obj, WorkSpace, HandsPlayer, MainObj, Worker;
    public bool CanWork, isSelling;
    private bool isIncreasing;
    public float TimeWork;

    public BuySpace space;
    public int lvl;
    public int[] PriceList;
    public Transform SaveStationPlace, WorkPlace, NewStationPlace;

    private GameObject meneger;

    void Start()
    {
        isIncreasing = true;
        HandsPlayer = GameObject.FindGameObjectWithTag("Hands");
    }

    // Update is called once per frame
    void Update()
    {
        CanWork = MainObj.activeSelf;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && lvl < 2)
        {
            HandsPlayer.SetActive(false);
            StartCoroutine(WorkSpaceEffect());
            WorkSpace.GetComponent<SpriteRenderer>().color = Color.green;
            TakeStationNew.CanGive = false;
            isSelling = true;
            if (station.CountObj >= 1 & newStation.CountObj < newStation.MuxCountObj) { StartCoroutine(Working()); }
        }

        if (other.gameObject.tag == "Worker")
        {
            StartCoroutine(WorkSpaceEffect());
            WorkSpace.GetComponent<SpriteRenderer>().color = Color.green;
            StartCoroutine(Working());
            isSelling = true;
        }

        if (other.GetComponent<Meneger>() != null && lvl < 2 && other.GetComponent<Meneger>().TargetType == "WorkWork" && !isSelling)
        {
            StartCoroutine(WorkSpaceEffect());
            StartCoroutine(Working());
            WorkSpace.GetComponent<SpriteRenderer>().color = Color.green;
            isSelling = true;
            meneger = other.gameObject;
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player" && lvl < 2 && meneger == null)
        {
            HandsPlayer.SetActive(true);
            TakeStationNew.CanGive = true;
            StopAllCoroutines();
            WorkSpace.GetComponent<SpriteRenderer>().color = Color.white;
            WorkSpace.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            isSelling = false;
        }

        if (other.GetComponent<Meneger>() != null && lvl < 2)
        {
            HandsPlayer.SetActive(true);
            TakeStationNew.CanGive = true;
            StopAllCoroutines();
            WorkSpace.GetComponent<SpriteRenderer>().color = Color.white;
            WorkSpace.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            isSelling = false;
        }
    }

    public IEnumerator Working()
    {
        yield return new WaitForSeconds(TimeWork);

        if (station.CountObj > 0 & newStation.CountObj < newStation.MuxCountObj)
        {
            Destroy(station.ObjectsList[station.CountObj - 1]);
            station.ObjectsList.RemoveAt(station.CountObj - 1);
            station.LustPosY -= 0.4f;
            station.CountObj--;

            Vector3 pos = new Vector3(newStation.PlaceObj.transform.position.x, newStation.LustPosY, newStation.PlaceObj.transform.position.z);
            GameObject newObj = Instantiate(obj, pos, obj.transform.rotation);
            newStation.ObjectsList.Add(newObj);
            newStation.LustPosY += 0.1f;
            newStation.CountObj++;
        }
        StartCoroutine(Working());
    }

    public IEnumerator WorkSpaceEffect()
    {
        if (isIncreasing)
        {
            WorkSpace.transform.localScale =
                new Vector3(WorkSpace.transform.localScale.x + 0.001f, WorkSpace.transform.localScale.y + 0.001f, WorkSpace.transform.localScale.z + 0.001f);
            if(WorkSpace.transform.localScale.x >= 0.13)
            {
                isIncreasing = false;
            }
        }
        else
        {
            WorkSpace.transform.localScale =
                            new Vector3(WorkSpace.transform.localScale.x - 0.001f, WorkSpace.transform.localScale.y - 0.001f, WorkSpace.transform.localScale.z - 0.001f);
            if (WorkSpace.transform.localScale.x <= 0.1)
            {
                isIncreasing = true;
            }
        }

        yield return new WaitForSeconds(0.01f);
        StartCoroutine(WorkSpaceEffect());
    }

    public void BuyUpgrade()
    {
        if (!MainObj.activeSelf)
        {
            MainObj.SetActive(true);
            space.ChangePrice(PriceList[lvl]);
        }
        else
        {
            if (lvl <= PriceList.Length)
            {
                station.MuxCountObj += 5;
                newStation.MuxCountObj += 5;
                TimeWork -= 0.5f;
            }
            if (lvl < PriceList.Length)
            {
                space.ChangePrice(PriceList[lvl]);
            }
        }
        if (lvl < 3) { lvl++; }
        if(lvl == 2) { Worker.SetActive(true); }
    }
}
