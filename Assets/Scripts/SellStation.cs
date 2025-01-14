using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellStation : MonoBehaviour
{
    public StationObjectCollection station;
    public GameObject WorkSpace;
    public Transform CustomerPlace;
    private bool isIncreasing;
    private PlayerWallet wallet;
    public int CountObj;
    public Customer client;
    void Start()
    {
        isIncreasing = true;
        wallet = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerWallet>();
    }

    void Update()
    {

    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            StartCoroutine(WorkSpaceEffect());
            StartCoroutine(Working());
            WorkSpace.GetComponent<SpriteRenderer>().color = Color.green;
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            StopAllCoroutines();
            WorkSpace.GetComponent<SpriteRenderer>().color = Color.white;
            WorkSpace.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        }
    }

    public IEnumerator Working()
    {
        Debug.Log("!");
        yield return new WaitForSeconds(0.3f);

        if (station.CountObj > 0 && client.BuyObjCount > 0)
        {
            Destroy(station.ObjectsList[station.CountObj - 1]);
            station.ObjectsList.RemoveAt(station.CountObj - 1);
            station.LustPosY -= 0.1f;
            station.CountObj--;
            wallet.ChangeCount(5, "Money");
            client.ChangeBuyCount();
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