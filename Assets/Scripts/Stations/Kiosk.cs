using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kiosk : MonoBehaviour
{
    public StationObjectCollection station1, station2, station3;
    public GameObject PresentCase, MaxTxt, Img1, Img2, ImgObj1, ImgObj2;
    public Transform menegerPlace;

    public List<GameObject> QueueList = new List<GameObject>();
    private bool inProgress;
    void Start()
    {
        
    }


    void Update()
    {
        Img1.transform.LookAt(Camera.main.transform.position);
        Img2.transform.LookAt(Camera.main.transform.position);
        if (station1.CountObj > 0 && ImgObj1.activeSelf) { ImgObj1.SetActive(false); }
        if (station2.CountObj > 0 && ImgObj2.activeSelf) { ImgObj2.SetActive(false); }


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
        if (station3.CountObj < station3.MuxCountObj && MaxTxt.activeSelf)
        {
            MaxTxt.SetActive(false);
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
        if(station1.CountObj == 0) { ImgObj1.SetActive(true); }
        if (station2.CountObj == 0) { ImgObj2.SetActive(true); }
        if (station3.CountObj == station3.MuxCountObj)
        {
            MaxTxt.SetActive(true);
        }
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
