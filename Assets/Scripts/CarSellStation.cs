using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSellStation : MonoBehaviour
{
    public StationObjectCollection station;
    public GameObject WorkSpace, MainObj, Worker;
    public Transform CustomerPlace;
    public bool CanWork;
    private bool isIncreasing;
    private PlayerWallet wallet;
    public CarCustomer client;
    public GameObject[] Queue;

    public BuySpace space;
    public int lvl;
    public int[] PriceList;

    void Start()
    {
        isIncreasing = true;
        wallet = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerWallet>();
    }

    void Update()
    {
        CanWork = MainObj.activeSelf;
    }

    public void OnTriggerEnter(Collider other)
    {
        if ((other.gameObject.tag == "Player" && lvl < 2) || (other.gameObject.tag == "Worker"))
        {
            StartCoroutine(WorkSpaceEffect());
            StartCoroutine(Working());
            WorkSpace.GetComponent<SpriteRenderer>().color = Color.green;
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player" && lvl < 2)
        {
            StopAllCoroutines();
            WorkSpace.GetComponent<SpriteRenderer>().color = Color.white;
            WorkSpace.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        }
    }

    public IEnumerator Working()
    {
        yield return new WaitForSeconds(0.3f);

        if (station.CountObj > 0 && client.BuyObjCount > 0)
        {
            Destroy(station.ObjectsList[station.CountObj - 1]);
            station.ObjectsList.RemoveAt(station.CountObj - 1);
            station.LustPosY -= 0.1f;
            station.CountObj--;
            wallet.ChangeCount(5, "Money");
            client.ChangeCountBuy();
        }
        StartCoroutine(Working());
    }

    public IEnumerator WorkSpaceEffect()
    {
        if (isIncreasing)
        {
            WorkSpace.transform.localScale =
                new Vector3(WorkSpace.transform.localScale.x + 0.001f, WorkSpace.transform.localScale.y + 0.001f, WorkSpace.transform.localScale.z + 0.001f);
            if (WorkSpace.transform.localScale.x >= 0.13)
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