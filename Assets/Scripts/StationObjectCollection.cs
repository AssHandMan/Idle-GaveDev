using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Collections.Specialized.BitVector32;

public class StationObjectCollection : MonoBehaviour
{

    public GameObject PlaceObj;
    public List<GameObject> ObjectsList;

    public int MuxCountObj, CountObj;
    public float LustPosY;

    void Start()
    {
        LustPosY = PlaceObj.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void RemoveAll()
    {
        for (int i = 0; i < ObjectsList.Count; i++)
        {
            Destroy(ObjectsList[i]);
            ObjectsList.RemoveAt(i);
            LustPosY -= 0.1f;
        }
    }

    public void OnDestroy()
    {
        RemoveAll();
    }
}
