using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationObjectCollection : MonoBehaviour
{

    public GameObject PlaceObj;
    public List<GameObject> ObjectsList;
    public AudioSource _audio;
    public bool _isToilet;
    public int MuxCountObj, CountObj;

    void Start()
    {

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
        }
    }

    public void OnDestroy()
    {
        RemoveAll();
        //if(_isToilet)
            //_audio.Play();
    }
}
