using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SellStation : MonoBehaviour
{
    public StationObjectCollection station;
    public GameObject WorkSpace, MainObj;
    public Transform[] CustomerPlace;
    public bool CanWork, isKiosk;
    public int CountObj;
    public Customer client;
    public List<GameObject> QueueList = new List<GameObject>();
    public GameObject[] Queue;

    private bool isIncreasing, inProgress;
    private PlayerWallet wallet;
    void Start()
    {
        isIncreasing = true;
        wallet = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerWallet>();
    }

    void Update()
    {
        CanWork = MainObj.activeSelf;
        if(isKiosk && client == null )
        {
            StopAllCoroutines();
            inProgress = false;
        }
        if(isKiosk && client != null && !inProgress) 
        {
            StartCoroutine(Working());
            inProgress = true;
        }
    }

    public void MakeArray()
    {
        Queue = QueueList.ToArray();
    }

    public void OnTriggerEnter(Collider other)
    {
        if(!isKiosk)
        {
            if (other.gameObject.tag == "Player")
            {
                StartCoroutine(WorkSpaceEffect());
                StartCoroutine(Working());
                WorkSpace.GetComponent<SpriteRenderer>().color = Color.green;
            }
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (!isKiosk)
        {
            if (other.gameObject.tag == "Player")
            {
                StopAllCoroutines();
                WorkSpace.GetComponent<SpriteRenderer>().color = Color.white;
                WorkSpace.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            }
        }
    }

    public IEnumerator Working()
    {
        Debug.Log("!");
        yield return new WaitForSeconds(0.3f);

        if (client != null)
        {
            if (station.CountObj > 0 && client.BuyObjCount > 0)
            {
                Destroy(station.ObjectsList[station.CountObj - 1]);
                station.ObjectsList.RemoveAt(station.CountObj - 1);
                station.LustPosY -= 0.1f;
                station.CountObj--;
                wallet.ChangeCount(5, "Money");
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
}