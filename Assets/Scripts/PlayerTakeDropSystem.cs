using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Collections.Specialized.BitVector32;

public class PlayerTakeDropSystem : MonoBehaviour
{
    public List<GameObject> IngredientsCollection;
    public int MaxCount;
    public int Count;

    public string ObjType;
    private TakeStation box;
    private DropStation drop;
    private GameObject plr, Bin, Delivery;

    void Start()
    {
        plr = GameObject.FindGameObjectWithTag("Player");
    }


    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E)) { TimeMagic(); }
    }

    public void TimeMagic()
    {
        if(Time.timeScale == 1)
        {
            Time.timeScale = 5;
        }
        else if (Time.timeScale == 5)
        {
            Time.timeScale = 1;
        }
    }

    public void TakeObject()
    {
        if (Count < MaxCount && box.CanGive)
        {
            if (ObjType == "" | ObjType == box.station.ObjectsList[0].GetComponent<Object>().Type)
            {
                Vector3 pos;
                if(Count == 0) { pos = transform.position; }
                else { pos = IngredientsCollection[IngredientsCollection.Count - 1].GetComponent<Object>().UpPos.position - box.station.ObjectsList[box.station.CountObj - 1].GetComponent<Object>().DownPos.localPosition; }
                GameObject newObj = Instantiate(box.station.ObjectsList[box.station.CountObj - 1], pos, transform.rotation);
                newObj.transform.parent = plr.transform;
                if (Count == 0) { ObjType = newObj.GetComponent<Object>().Type; }
                IngredientsCollection.Add(newObj);
                Count++;
                box.DeleteObj();
            }
        }
    }

    private IEnumerator AddingObjectProc()
    {
        TakeObject();
        yield return new WaitForSeconds(0.2f);
        if (box != null)
        {
            StartCoroutine(AddingObjectProc());
        }
        else
        {
            StopAllCoroutines();
        }
    }

    public void DropObject()
    {
        if (Count > 0 && drop != null && drop.station.CountObj < drop.station.MuxCountObj)
        {
            if (ObjType == drop.ObjectType)
            {
                drop.AddObj(IngredientsCollection[Count - 1]);
                Destroy(IngredientsCollection[Count - 1]);
                IngredientsCollection.RemoveAt(Count - 1);
                Count--;
                if (Count == 0) { ObjType = ""; }
            }
        }
        else
        {
            if(Bin != null && Count > 0)
            {
                Destroy(IngredientsCollection[Count - 1]);
                IngredientsCollection.RemoveAt(Count - 1);
                Count--;
                if (Count == 0) { ObjType = ""; }
            }
            if (Delivery != null && Delivery.GetComponent<DeliveryMan>().OrderBar.activeSelf && Count > 0 && IngredientsCollection[0].GetComponent<Object>().Type == Delivery.GetComponent<DeliveryMan>().Type)
            {
                if (Delivery.GetComponent<DeliveryMan>().BuyObjCount > 0)
                {
                    Destroy(IngredientsCollection[Count - 1]);
                    IngredientsCollection.RemoveAt(Count - 1);
                    Count--;
                    Delivery.GetComponent<DeliveryMan>().AddToOrder();
                    if (Count == 0) { ObjType = ""; }
                }
            }
        }
    }

    private IEnumerator DroppingObjectProc()
    {
        DropObject();
        yield return new WaitForSeconds(0.2f);
        if (drop != null || Bin != null || (Delivery != null && Delivery.GetComponent<DeliveryMan>().OrderBar.activeSelf))
        {
            StartCoroutine(DroppingObjectProc());
        }
        else
        {
            StopAllCoroutines();
        }
    }

    public void Take()
    {
        if (box != null && box.CanGive)
        {
            TakeObject();
        }
    }


    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<TakeStation>() != null)
        {
            if (other.gameObject.tag != "Kiosk")
            {
                box = other.gameObject.GetComponent<TakeStation>();
                StartCoroutine(AddingObjectProc());
            }
        }
        if (other.gameObject.GetComponent<DropStation>() != null)
        {
            drop = other.gameObject.GetComponent<DropStation>();
            StartCoroutine(DroppingObjectProc());
        }
        if(other.gameObject.tag == "Trashbin") 
        {
            Bin = other.gameObject;
            StartCoroutine(DroppingObjectProc());
        }
        if (other.GetComponent<DeliveryMan>() != null)
        {
            Delivery = other.gameObject;
            StartCoroutine(DroppingObjectProc());
        }
    }



    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<TakeStation>() != null)
        {
            if (other.gameObject.tag != "Kiosk")
            {
                box = null;
                StopAllCoroutines();
            }
        }
        if (other.gameObject.GetComponent<DropStation>() != null)
        {
            drop = null;
            StopAllCoroutines();
        }
        if (other.gameObject.tag == "Trashbin")
        {
            Bin = null;
            StopAllCoroutines();
        }
        if (other.GetComponent<DeliveryMan>() != null)
        {
            Delivery = null;
            StopAllCoroutines();
        }
    }
}