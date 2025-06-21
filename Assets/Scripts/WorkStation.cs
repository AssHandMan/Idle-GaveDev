using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkStation : MonoBehaviour
{
    public StationObjectCollection station, newStation;
    public TakeStation TakeStationNew;
    public GameObject obj, WorkSpace, MainObj, Worker, MaxTxt;
    public bool CanWork, isSelling;
    private bool isIncreasing;
    public float TimeWork;

    public BuySpace space;
    public int lvl;
    public int[] PriceList;
    public Transform SaveStationPlace, WorkPlace, NewStationPlace;

    private GameObject meneger, player;

    void Start()
    {
        isIncreasing = true;
    }

    void Update()
    {
        CanWork = MainObj.activeSelf;
        if (newStation.CountObj < newStation.MuxCountObj && MaxTxt.activeSelf)
        {
            MaxTxt.SetActive(false);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && lvl < 2)
        {
            if (meneger == null && lvl < 2) StartCoroutine(WorkSpaceEffect());
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

        if (other.GetComponent<Meneger>() != null && lvl < 2 && other.GetComponent<Meneger>().TargetType == "WorkWork")
        {
            if (!isSelling) { StartCoroutine(WorkSpaceEffect()); }
            StartCoroutine(Working());
            WorkSpace.GetComponent<SpriteRenderer>().color = Color.green;
            isSelling = true;
            meneger = other.gameObject;
        }
    }

    public void OnTriggerStay(Collider other)
    {
        
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player" && lvl < 2 && meneger == null)
        {
            TakeStationNew.CanGive = true;
            StopAllCoroutines();
            WorkSpace.GetComponent<SpriteRenderer>().color = Color.white;
            WorkSpace.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            isSelling = false;
        }

        if (other.GetComponent<Meneger>() != null && lvl < 2)
        {
            TakeStationNew.CanGive = true;
            StopAllCoroutines();
            WorkSpace.GetComponent<SpriteRenderer>().color = Color.white;
            WorkSpace.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            isSelling = false;
            meneger = null;
        }
    }

    public IEnumerator Working()
    {
        yield return new WaitForSeconds(TimeWork);

        if (station.CountObj > 0 & newStation.CountObj < newStation.MuxCountObj)
        {
            Destroy(station.ObjectsList[station.CountObj - 1]);
            station.ObjectsList.RemoveAt(station.CountObj - 1);
            station.CountObj--;

            Vector3 pos;
            if (newStation.CountObj == 0) { pos = newStation.PlaceObj.transform.position; }
            else { pos = newStation.ObjectsList[newStation.ObjectsList.Count - 1].GetComponent<Object>().UpPos.position - obj.GetComponent<Object>().DownPos.localPosition; }
            GameObject newObj = Instantiate(obj, pos, obj.transform.rotation);
            newStation.ObjectsList.Add(newObj);
            newStation.CountObj++;
            if(newStation.CountObj == newStation.MuxCountObj)
            {
                MaxTxt.SetActive(true);
            }
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
        transform.parent.gameObject.GetComponent<Animator>().SetTrigger("Buy");
    }
}
