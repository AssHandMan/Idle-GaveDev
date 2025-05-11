using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcadeMachine : MonoBehaviour
{
    public StationObjectCollection station;
    public MeshFilter filter;
    public GameObject trash, TrashViewe, Main;
    public Transform CustomerPos;
    public Customer client;
    public ParticleSystem playEffect;
    public bool isPlaying, hasTarget, CanWork, isToilet, hasMenegerTarget, isPause;
    [SerializeField] private GameObject _stackOfMoney;

    public int lvl;
    public Mesh[] BoxLvl;
    public BuySpace space;
    public int[] PriceList;
    private int GameTime;

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
        if(isToilet) { client.ToiletIcon.SetActive(true); }
        client.meneger.ReadyforSleep.Add(client);
        isPlaying = true;
        StartCoroutine(Playing());
        GameTime = 5;
        if (!isToilet) { StartCoroutine(PlayEffect()); }
    }
    private IEnumerator PlayEffect()
    {
        while (isPause)
        {
            yield return null;
            playEffect.Stop();
        }
        playEffect.Play();
        yield return new WaitForSeconds(1);
        if(client != null)
        {
            StartCoroutine(PlayEffect());
        }
    }
    private IEnumerator Playing()
    {
        while (isPause)
        {
            yield return null;
        }
        yield return new WaitForSeconds(1);
        GameTime--;
        if(GameTime > 0)
        {
            StartCoroutine(Playing());
        }
        else
        {
            EndGame();
        }
        
    }
    private void EndGame()
    {
        PlayerWallet plr = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerWallet>();
        _stackOfMoney.GetComponent<MoneyZoneController>().Add();
        client.play = true;
        client.GoAway();
        client.meneger.ReadyforSleep.Remove(client);
        client = null;
        Vector3 pos;
        if (station.CountObj == 0) { pos = station.PlaceObj.transform.position; }
        else { pos = station.ObjectsList[station.ObjectsList.Count - 1].GetComponent<Object>().UpPos.position - trash.GetComponent<Object>().DownPos.localPosition; }
        GameObject newObj = Instantiate(trash, pos, transform.rotation);
        station.ObjectsList.Add(newObj);
        station.CountObj++;
        CanWork = false;
        isPlaying = false;
        hasTarget = false;
        if (isToilet)
        {
            TrashViewe.SetActive(true);
        }
    }
    public void BuyUpgrade()
    {
        GetComponent<Animator>().SetTrigger("Buy");
        if (lvl <= PriceList.Length)
        {
            if (BoxLvl[lvl] != null) { filter.mesh = BoxLvl[lvl]; }
        }
        if(lvl < PriceList.Length)
        {
            space.ChangePrice(PriceList[lvl]);
        }
        if (lvl < 3) { lvl++; }
    }
}
