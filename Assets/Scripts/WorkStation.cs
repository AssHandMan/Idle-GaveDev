using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkStation : MonoBehaviour
{
    public StationObjectCollection station, newStation;
    public TakeStation TakeStationNew;
    public GameObject obj, WorkSpace, HandsPlayer, MainObj;
    private bool isIncreasing;
    public float TimeWork;

    public BuySpace space;
    public int lvl;
    public int[] PriceList;

    void Start()
    {
        isIncreasing = true;
        HandsPlayer = GameObject.FindGameObjectWithTag("Hands");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            HandsPlayer.SetActive(false);
            StartCoroutine(WorkSpaceEffect());
            WorkSpace.GetComponent<SpriteRenderer>().color = Color.green;
            TakeStationNew.CanGive = false;
            if (station.CountObj >= 1 & newStation.CountObj < newStation.MuxCountObj) { StartCoroutine(Working()); }   
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            HandsPlayer.SetActive(true);
            TakeStationNew.CanGive = true;
            StopAllCoroutines();
            WorkSpace.GetComponent<SpriteRenderer>().color = Color.white;
            WorkSpace.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
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
    }
}
