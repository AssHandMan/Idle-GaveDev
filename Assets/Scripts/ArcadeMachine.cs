using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcadeMachine : MonoBehaviour
{
    public StationObjectCollection station;
    public GameObject trash, TrashViewe, Main;
    public Transform CustomerPos;
    public Customer client;
    public ParticleSystem playEffect;
    public bool isPlaying, hasTarget, CanWork, isToilet, hasMenegerTarget;
    [SerializeField] private GameObject _stackOfMoney;

    public int lvl;
    public Mesh[] BoxLvl;
    public BuySpace space;
    public int[] PriceList;

    void Start()
    {
        if (!isToilet) { CanWork = true; }
        else
        {
            CanWork = Main.activeSelf;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(station.CountObj == 0) 
        { 
            if(isToilet)
            {
                TrashViewe.SetActive(false);
                if(Main.activeSelf) { CanWork = true; }
            }
            else
            {
                CanWork = true;
                hasMenegerTarget = false;
            }
        }
    }

    public void StartPlay()
    {
        isPlaying = true;
        StartCoroutine(Playing());
        StartCoroutine(PlayEffect());
    }
    private IEnumerator PlayEffect()
    {
        playEffect.Play();
        yield return new WaitForSeconds(1);
        if(client != null )
        {
            StartCoroutine(PlayEffect());
        }
    }
    private IEnumerator Playing()
    {
        yield return new WaitForSeconds(5);
        PlayerWallet plr = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerWallet>();
        //изменено на кучу
        //plr.ChangeCount(10, "Money");
        _stackOfMoney.GetComponent<MoneyZoneController>().Add();
        client.play = true;
        client.GoAway();
        client = null;
        Vector3 pos;
        if (station.CountObj == 0) { pos = station.PlaceObj.transform.position; }
        else { pos = station.ObjectsList[station.ObjectsList.Count - 1].GetComponent<Object>().UpPos.position - trash.GetComponent<Object>().DownPos.localPosition; }
        GameObject newObj = Instantiate(trash, pos, transform.rotation);
        //newObj.transform.parent = transform;
        station.ObjectsList.Add(newObj);
        station.CountObj++;
        CanWork = false;
        isPlaying = false;
        hasTarget = false;
        if(isToilet)
        {
            TrashViewe.SetActive(true);
        }
    }
    public void BuyUpgrade()
    {
        if (lvl <= PriceList.Length)
        {
            if (BoxLvl[lvl] != null) { gameObject.GetComponent<MeshFilter>().mesh = BoxLvl[lvl]; }
        }
        if(lvl < PriceList.Length)
        {
            space.ChangePrice(PriceList[lvl]);
        }
        if (lvl < 3) { lvl++; }
    }
}
