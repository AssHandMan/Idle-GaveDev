using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Collections.Specialized.BitVector32;

public class ArcadeMachine : MonoBehaviour
{
    public StationObjectCollection station;
    public GameObject trash;
    public Transform CustomerPos;
    public Customer client;
    public bool isPlaying, hasTarget, CanWork;

    void Start()
    {
        CanWork = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(station.CountObj == 0) { CanWork = true; }
    }

    public void StartPlay()
    {
        isPlaying = true;
        StartCoroutine(Playing());
    }
    private IEnumerator Playing()
    {
        yield return new WaitForSeconds(5);
        PlayerWallet plr = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerWallet>();
        plr.ChangeCount(10, "Money");
        client.play = true;
        client.GoAway();
        client = null;
        Vector3 pos = new Vector3(station.PlaceObj.transform.position.x, station.LustPosY, station.PlaceObj.transform.position.z);
        GameObject newObj = Instantiate(trash, pos, transform.rotation);
        newObj.transform.parent = transform;
        station.ObjectsList.Add(newObj);
        station.LustPosY += 0.1f;
        station.CountObj++;
        CanWork = false;
        isPlaying = false;
        hasTarget = false;
    }
}
