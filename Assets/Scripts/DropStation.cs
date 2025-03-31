using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropStation : MonoBehaviour
{
    public StationObjectCollection station;
    public string ObjectType;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddObj(GameObject obj)
    {
        if (obj.GetComponent<Object>().Type == ObjectType)
        {
            Vector3 pos;
            if (station.CountObj == 0) { pos = station.PlaceObj.transform.position; }
            else { pos = station.ObjectsList[station.ObjectsList.Count - 1].GetComponent<Object>().UpPos.position - obj.GetComponent<Object>().DownPos.localPosition; }
            GameObject newObj = Instantiate(obj, pos, station.PlaceObj.transform.rotation);
            newObj.transform.parent = transform;
            station.ObjectsList.Add(newObj);
            station.CountObj++;
        }
    }
}