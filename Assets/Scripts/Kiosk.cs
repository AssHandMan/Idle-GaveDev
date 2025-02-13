using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kiosk : MonoBehaviour
{
    public StationObjectCollection station1, station2, station3;
    public GameObject PresentCase;

    public List<GameObject> QueueList = new List<GameObject>();
    private bool inProgress;
    void Start()
    {
        
    }


    void Update()
    {
        if (station1.CountObj > 0 && station2.CountObj > 0)
        {
            if (station3.CountObj < station3.MuxCountObj)
            {
                if (!inProgress)
                {
                    StartCoroutine(MakePresentBox());
                    inProgress = true;
                }
            }
        }
    }

    private IEnumerator MakePresentBox()
    {
        yield return new WaitForSeconds(0.5f);
        station1.GetComponent<TakeStation>().DeleteObj();
        station2.GetComponent<TakeStation>().DeleteObj();
        Vector3 pos = new Vector3(station3.PlaceObj.transform.position.x, station3.LustPosY, station3.PlaceObj.transform.position.z);
        GameObject newObj = Instantiate(PresentCase, pos, transform.rotation);
        newObj.transform.parent = station3.transform;
        station3.ObjectsList.Add(newObj);
        station3.LustPosY += 0.1f;
        station3.CountObj++;
        
        if (station1.CountObj > 0 && station2.CountObj > 0)
        {
            if (station3.CountObj < station3.MuxCountObj)
            {
                StartCoroutine(MakePresentBox());
            }
        }
        else
        {
            inProgress = false;
        }
    }
}
