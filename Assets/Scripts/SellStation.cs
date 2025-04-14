using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class SellStation : MonoBehaviour
{
    public StationObjectCollection station;
    public GameObject WorkSpace, MainObj, Worker;
    public Transform[] CustomerPlace;
    public bool CanWork, isKiosk, isCalling, isSelling, hasAddingObjMenegerTarget, hasworkerTarget;
    public int CountObj;
    public float deltaY;
    public Customer client;
    public List<GameObject> QueueList = new List<GameObject>();
    public GameObject[] Queue;
    public Customer CallingCustomer, LustCustomer;
    private bool isIncreasing, inProgress, withMeneger;
    private PlayerWallet wallet;
    private Meneger meneger;

    public Transform MenegerPlace;
    public BuySpace space;
    public int lvl;
    public int[] PriceList;

    [SerializeField] private GameObject _stackOfMoney;
    void Start()
    {
        isIncreasing = true;
        wallet = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerWallet>();
    }

    void Update()
    {
        CanWork = MainObj.activeSelf;
        if (isKiosk && client == null)
        {
            StopAllCoroutines();
            inProgress = false;
        }
        if (isKiosk && client != null && !inProgress)
        {
            StartCoroutine(Working());
            inProgress = true;
        }

        if (Queue.Length == 10)
        {
            if (LustCustomer == null)
            {
                LustCustomer = Queue[9].GetComponent<Customer>();
            }
        }
    }

    public void MakeArray()
    {
        Queue = QueueList.ToArray();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (!isKiosk)
        {
            if (other.gameObject.tag == "Player" && lvl < 2)
            {
                if(meneger == null && lvl < 2) StartCoroutine(WorkSpaceEffect());
                StartCoroutine(Working());
                WorkSpace.GetComponent<SpriteRenderer>().color = Color.green;
                isSelling = true;
            }

            if (other.gameObject.tag == "Worker")
            {
                StartCoroutine(WorkSpaceEffect());
                StartCoroutine(Working());
                WorkSpace.GetComponent<SpriteRenderer>().color = Color.green;
                isSelling = true;
            }
            if(other.GetComponent<Meneger>() != null)
            {
                StartCoroutine(WorkSpaceEffect());
                StartCoroutine(Working());
                WorkSpace.GetComponent<SpriteRenderer>().color = Color.green;
                isSelling = true;
                withMeneger = true;
                meneger = other.GetComponent<Meneger>();
            }
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (!isKiosk)
        {
            if (other.gameObject.tag == "Player" && lvl < 2 && !withMeneger)
            {
                StopAllCoroutines();
                WorkSpace.GetComponent<SpriteRenderer>().color = Color.white;
                WorkSpace.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                isSelling = false;
            }
            if (other.GetComponent<Meneger>() != null && lvl < 2)
            {
                StopAllCoroutines();
                WorkSpace.GetComponent<SpriteRenderer>().color = Color.white;
                isSelling = false;
                withMeneger = false;
                meneger = null;
            }
        }
    }

    public IEnumerator Working()
    {

        yield return new WaitForSeconds(0.3f);

        if (client != null)
        {
            if (station.CountObj > 0 && client.BuyObjCount > 0 && !client.isCalling)
            {
                Destroy(station.ObjectsList[station.CountObj - 1]);
                station.ObjectsList.RemoveAt(station.CountObj - 1);
                station.CountObj--;
                //wallet.ChangeCount(5, "Money");//изменить
                _stackOfMoney.GetComponent<MoneyZoneController>().Add();
                client.ChangeBuyCount();
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
        if(!MainObj.activeSelf)
        {
            MainObj.SetActive(true);
            if(!isKiosk)space.ChangePrice(PriceList[lvl]);
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