using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MenegerHands : MonoBehaviour
{
    public Meneger meneger;
    public List<GameObject> IngredientsCollection;
    public int Count;
    public float LustPosY;

    private CreateStation createStation;
    public DropStation dropStation;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if(meneger.TargetType == "Arcade" && other.GetComponent<ArcadeMachine>() != null)
        {
            ArcadeMachine arcade = other.GetComponent<ArcadeMachine>();
            if(meneger.Target == arcade.CustomerPos)
            {
                if (arcade.station.CountObj != 0)
                {
                    Vector3 pos = new Vector3(transform.position.x, LustPosY, transform.position.z);
                    GameObject newObj = Instantiate(arcade.station.ObjectsList[arcade.station.CountObj - 1], pos, transform.rotation);
                    newObj.transform.parent = transform;
                    IngredientsCollection.Add(newObj);
                    LustPosY += 0.1f;
                    Count++;
                    arcade.GetComponent<TakeStation>().DeleteObj();
                    arcade.hasMenegerTarget = false;
                }
            }
        }
        else if (meneger.TargetType == "Delete" && other.GetComponent<Trashbin>() != null)
        {
            if (meneger.Target == other.GetComponent<Trashbin>().MenegerPlace)
            {
                Destroy(IngredientsCollection[Count - 1]);
                IngredientsCollection.RemoveAt(Count - 1);
                LustPosY -= 0.1f;
                Count--;
            }
        }
        else if (meneger.TargetType == "CreateStation" && other.GetComponent<CreateStation>() != null)
        {
            if (meneger.Target == other.GetComponent<CreateStation>().MenegerPlace)
            {
                createStation = other.GetComponent<CreateStation>();
                StartCoroutine(GetFromCreateStation());
            }
        }
        else if (meneger.TargetType == "AddToCashmachine" && other.GetComponent<DropStation>() != null)
        {
            dropStation = other.GetComponent<DropStation>();
            StartCoroutine(AddToCashMachine());
        }
    }

    private IEnumerator GetFromCreateStation()
    {
        yield return new WaitForSeconds(0.2f);
        if(createStation.station.CountObj == 0) { StartCoroutine(WaitNewCreateObj()); }
        else if((meneger.capacity > meneger.Cashmachine.station.MuxCountObj && Count < meneger.Cashmachine.station.MuxCountObj) || (meneger.capacity < meneger.Cashmachine.station.MuxCountObj && Count < meneger.capacity))
        {
            Vector3 pos = new Vector3(transform.position.x, LustPosY, transform.position.z);
            GameObject newObj = Instantiate(createStation.station.ObjectsList[createStation.station.CountObj - 1], pos, transform.rotation);
            newObj.transform.parent = transform;
            IngredientsCollection.Add(newObj);
            LustPosY += 0.1f;
            Count++;
            createStation.GetComponent<TakeStation>().DeleteObj();

            if ((meneger.capacity > meneger.Cashmachine.station.MuxCountObj && Count < meneger.Cashmachine.station.MuxCountObj) || (meneger.capacity < meneger.Cashmachine.station.MuxCountObj && Count < meneger.capacity)) { StartCoroutine(GetFromCreateStation()); }   
        }
    }

    private IEnumerator WaitNewCreateObj()
    {
        yield return new WaitForSeconds(0.2f);
        StartCoroutine(GetFromCreateStation());
    }

    private IEnumerator AddToCashMachine()
    {
        yield return new WaitForSeconds(0.2f);
        if (Count > 0 && dropStation.station.CountObj < dropStation.station.MuxCountObj)
        {
            dropStation.AddObj(IngredientsCollection[Count - 1]);

            Destroy(IngredientsCollection[Count - 1]);
            IngredientsCollection.RemoveAt(Count - 1);
            LustPosY -= 0.1f;
            Count--;
            if (Count > 0 && dropStation.station.CountObj < dropStation.station.MuxCountObj) { StartCoroutine(AddToCashMachine()); }
        }
    }
}
