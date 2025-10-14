using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellStation : MonoBehaviour
{
    public StationObjectCollection station;
    public GameObject WorkSpace, MainObj, Worker, LustCustomer, NoSeatTable;
    public Transform[] CustomerPlace;
    public bool CanWork, isKiosk, isCalling, isSelling, hasAddingObjMenegerTarget, hasworkerTarget;
    public int CountObj;
    public float deltaY;
    public Customer client;
    public List<GameObject> QueueList = new List<GameObject>();
    public GameObject[] Queue;
    public Customer CallingCustomer;
    private bool isIncreasing, inProgress, withMeneger;
    private PlayerWallet wallet;
    private Meneger meneger;

    public Transform MenegerPlace;
    public BuySpace space;
    public int lvl, LustCustomerPosinQueue;
    public int[] PriceList;
    private List<GameObject> PeopleInWorkZone;

    [SerializeField] private GameObject _stackOfMoney;
    [SerializeField] private ParticleSystem circleArea;

    [Header("Audio")]
    public AudioClip _clip;
    public AudioSource _audio;
    public float _volume = 0.4f;

    void Start()
    {
        isIncreasing = true;
        wallet = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerWallet>();
        PeopleInWorkZone = new List<GameObject>();
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
        if (!isKiosk)
        {
            if (NoSeatTable.activeSelf)
            {
                NoSeatTable.transform.LookAt(Camera.main.transform);
            }
        }
    }
    public void CheckCalling()
    {
        int flag = 13;
        Debug.Log("CheckCalling");
        foreach (GameObject obj in QueueList)
        {
            if (obj.GetComponent<Customer>().isCalling)
            {
                flag = obj.GetComponent<Customer>().ppos;
                Debug.Log("flag " + flag.ToString());
            }
        }
        foreach (GameObject obj in QueueList)
        {
            Debug.Log(obj.GetComponent<Customer>().ppos.ToString()+" vs " + flag.ToString());

            if (obj.GetComponent<Customer>().ppos > flag)
            {
                Debug.Log(obj.GetComponent<Customer>().ppos);
                obj.GetComponent<Customer>()._animator.SetBool("isAngry", true);
            }
        }
    }
    public void UnCalling()
    {
        foreach (GameObject obj in QueueList)
        {
            obj.GetComponent<Customer>()._animator.SetBool("isAngry", false);
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
            if (other.gameObject.tag == "Player" || other.gameObject.tag == "Worker" || other.GetComponent<Meneger>() != null)
            {
                PeopleInWorkZone.Add(other.gameObject);
                if (!isSelling)
                {
                    StartCoroutine(WorkSpaceEffect());
                    StartCoroutine(Working());
                    WorkSpace.GetComponent<SpriteRenderer>().color = Color.green;
                    isSelling = true;
                }
                if (other.GetComponent<Meneger>() != null) { withMeneger = true; meneger = other.GetComponent<Meneger>(); }
            }
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (!isKiosk)
        {
            if (other.gameObject.tag == "Player" || other.gameObject.tag == "Worker" || other.GetComponent<Meneger>() != null)
            {
                if (other.GetComponent<Meneger>() != null) { withMeneger = false; meneger = null; }
                PeopleInWorkZone.Remove(other.gameObject);
                if (PeopleInWorkZone.Count == 0 && lvl < 2)
                {
                    StopAllCoroutines();
                    WorkSpace.GetComponent<SpriteRenderer>().color = Color.white;
                    WorkSpace.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                    isSelling = false;
                }
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
                _audio.PlayOneShot(_clip, _volume);
                station.ObjectsList.RemoveAt(station.CountObj - 1);
                station.CountObj--;
                //wallet.ChangeCount(5, "Money");//изменить
                _stackOfMoney.GetComponent<MoneyZoneController>().Add();
                client.ChangeBuyCount();
            }
            
        }
        StartCoroutine(Working());
    }
    public void SetNoSeatTable(bool isActive) { NoSeatTable.SetActive(isActive); }

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
            if (!isKiosk) space.ChangePrice(PriceList[lvl]);
            else { circleArea.Play(); }
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
        if (lvl == 2) 
        { 
            Worker.SetActive(true);
            StartCoroutine(WorkSpaceEffect());
            StartCoroutine(Working());
            WorkSpace.GetComponent<SpriteRenderer>().color = Color.green;
            isSelling = true;
        }
        if (!isKiosk) { transform.parent.gameObject.GetComponent<Animator>().SetTrigger("Buy"); }
        else 
        {
            Transform topParent = transform;
            for(int i = 0; i <2; i++) { topParent = topParent.parent; }
            topParent.gameObject.GetComponent<Animator>().SetTrigger("Buy");
        }
        circleArea.Play();
    }
}