using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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
            Vector3 pos = new Vector3(station.PlaceObj.transform.position.x, station.LustPosY, station.PlaceObj.transform.position.z);
            GameObject newObj = Instantiate(obj, pos, transform.rotation);
            newObj.transform.parent = transform;
            station.ObjectsList.Add(newObj);
            station.LustPosY += 0.1f;
            station.CountObj++;
        }
    }
}