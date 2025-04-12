using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kiosk : MonoBehaviour
{
    public StationObjectCollection station1, station2, station3;
    public GameObject PresentCase;
    public Transform menegerPlace;

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
        Vector3 pos;
        if (station3.CountObj == 0) { pos = station3.PlaceObj.transform.position; }
        else { pos = station3.ObjectsList[station3.ObjectsList.Count - 1].GetComponent<Object>().UpPos.position - PresentCase.GetComponent<Object>().DownPos.localPosition; }
        GameObject newObj = Instantiate(PresentCase, pos, transform.rotation);
        newObj.transform.parent = station3.transform;
        station3.ObjectsList.Add(newObj);
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
