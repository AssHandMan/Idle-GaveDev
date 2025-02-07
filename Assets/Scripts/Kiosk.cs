using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kiosk : MonoBehaviour
{
    public StationObjectCollection station1, station2, station3;
    public GameObject PresentCase;
    void Start()
    {
        
    }


    void Update()
    {
        if (station1.CountObj == 1 && station2.CountObj == 1)
        {
            if (station3.CountObj < 1)
            {
                station1.RemoveAll();
                station2.RemoveAll();
                Vector3 pos = new Vector3(station3.PlaceObj.transform.position.x, station3.LustPosY, station3.PlaceObj.transform.position.z);
                GameObject newObj = Instantiate(PresentCase, pos, transform.rotation);
                newObj.transform.parent = station3.transform;
                station3.ObjectsList.Add(newObj);
                station3.LustPosY += 0.1f;
                station3.CountObj++;
            }
        }
    }
}
