using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropStation : MonoBehaviour
{
    public StationObjectCollection station;
    public string ObjectType;

    public bool inProgress;
    private Vector3 startPos;
    private Vector3 endPos;
    private Vector3 controlPoint;
    private float arcT = 0f;
    private float TakeSpeed;
    private GameObject LustTakenObj;

    void Start()
    {
        
    }

    void Update()
    {
        if (inProgress)
        {
            if (LustTakenObj == null)
            {
                inProgress = false;
                return;
            }

            arcT += Time.deltaTime * TakeSpeed;
            arcT = Mathf.Clamp01(arcT);
            if (station.CountObj == 0)
            {
                endPos = station.PlaceObj.transform.position;
            }
            else
            {
                station.ObjectsList.RemoveAll(x => x == null);
                if (station.ObjectsList.Count == 0) { endPos = station.PlaceObj.transform.position; }
                else { endPos = station.ObjectsList[station.ObjectsList.Count - 1].GetComponent<Object>().UpPos.position - LustTakenObj.GetComponent<Object>().DownPos.localPosition; }
            }
            Vector3 pos = Mathf.Pow(1 - arcT, 2) * startPos + 2 * (1 - arcT) * arcT * controlPoint + Mathf.Pow(arcT, 2) * endPos;
            LustTakenObj.transform.position = pos;

            if (arcT >= 1f)
            {
                LustTakenObj.transform.position = endPos;
                LustTakenObj.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
                station.ObjectsList.Add(LustTakenObj);
                station.CountObj++;
                inProgress = false;
            }
        }
    }

    public void AddObj(GameObject obj, float speed)
    {
        if (obj.GetComponent<Object>().Type == ObjectType && !inProgress)
        {
            TakeSpeed = speed;
            obj.transform.parent = null;
            startPos = obj.transform.position;
            arcT = 0;
            if (station.CountObj == 0) { endPos = station.PlaceObj.transform.position; }
            else { endPos = station.ObjectsList[station.ObjectsList.Count - 1].GetComponent<Object>().UpPos.position - obj.GetComponent<Object>().DownPos.localPosition; }

            controlPoint = (startPos + endPos) / 2f;
            controlPoint.y += 3f;

            LustTakenObj = obj;
            inProgress = true;
        }
    }
}