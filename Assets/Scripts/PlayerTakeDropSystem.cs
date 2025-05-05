using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTakeDropSystem : MonoBehaviour
{
    public List<GameObject> IngredientsCollection;
    public int MaxCount;
    public int Count;
    public float TakeSpeed;
    public ArrowPosController arrow;

    public string ObjType;
    private TakeStation box;
    private DropStation drop;
    private GameObject plr, Bin, Delivery;

    private bool inProgress;
    private int CountForLustObj;
    private GameObject LustTakenObj, PrevTakenObj;

    private Vector3 startPos;
    private Vector3 endPos;
    private Vector3 controlPoint;
    private float arcT = 0f;

    void Start()
    {
        plr = GameObject.FindGameObjectWithTag("Player");
    }


    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E)) { TimeMagic(); }
        if (inProgress)
        {
            arcT += Time.deltaTime * TakeSpeed;
            arcT = Mathf.Clamp01(arcT);

            if (CountForLustObj == 0) { endPos = transform.position; }
            else { endPos = PrevTakenObj.GetComponent<Object>().UpPos.position - LustTakenObj.GetComponent<Object>().DownPos.localPosition; }

            controlPoint = (startPos + endPos) / 2f;
            controlPoint.y += 3f;

            Vector3 pos = Mathf.Pow(1 - arcT, 2) * startPos + 2 * (1 - arcT) * arcT * controlPoint + Mathf.Pow(arcT, 2) * endPos;
            LustTakenObj.transform.position = pos;

            if (arcT >= 1f)
            {
                LustTakenObj.transform.position = endPos;
                LustTakenObj.transform.parent = transform;
                LustTakenObj.transform.rotation = transform.rotation;
                inProgress = false;
            }
        }
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
        if (Count < MaxCount && box.CanGive && !inProgress)
        {
            if (ObjType == "" | ObjType == box.station.ObjectsList[0].GetComponent<Object>().Type)
            {
                GameObject newObj = box.station.ObjectsList[box.station.ObjectsList.Count - 1];
                if (Count == 0) { ObjType = newObj.GetComponent<Object>().Type; }
                box.RemoveObj();

                CountForLustObj = Count;
                GameObject prevObj = (Count > 0) ? IngredientsCollection[IngredientsCollection.Count - 1] : null;
                PrevTakenObj = prevObj;
                LustTakenObj = newObj;
                startPos = newObj.transform.position;
                arcT = 0f;

                inProgress = true;
                IngredientsCollection.Add(newObj);
                Count++;
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
        if (Count > 0 && drop != null && drop.station.CountObj < drop.station.MuxCountObj && !drop.inProgress)
        {
            if (ObjType == drop.ObjectType)
            {
                drop.AddObj(IngredientsCollection[Count - 1], TakeSpeed);
                //Destroy(IngredientsCollection[Count - 1]);
                IngredientsCollection.RemoveAt(Count - 1);
                Count--;
                if (Count == 0) { ObjType = ""; }
            }
        }
        else
        {
            if(Bin != null && Count > 0 && !Bin.GetComponent<Trashbin>().inProgress)
            {
                if(arrow.posLvl == 5 && IngredientsCollection[0].GetComponent<Object>().Type == "Trash")
                {
                    arrow.inStationPosesProgress = false;
                    arrow.MoveNextToBuySpace();
                }
                Bin.GetComponent<Trashbin>().AddObj(IngredientsCollection[Count - 1], TakeSpeed);
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