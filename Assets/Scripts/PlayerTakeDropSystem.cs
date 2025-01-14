using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTakeDropSystem : MonoBehaviour
{
    public List<GameObject> IngredientsCollection;
    public int MaxCount;
    public int Count;
    public float LustPosY;

    public string ObjType;
    private TakeStation box;
    private DropStation drop;
    private GameObject plr, Bin;

    void Start()
    {
        LustPosY = transform.position.y;
        plr = GameObject.FindGameObjectWithTag("Player");
    }


    void Update()
    {

    }

    public void TakeObject()
    {
        if (Count < MaxCount && box.CanGive)
        {
            if (ObjType == "" | ObjType == box.station.ObjectsList[0].GetComponent<Object>().Type)
            {
                Vector3 pos = new Vector3(transform.position.x, LustPosY, transform.position.z);
                GameObject newObj = Instantiate(box.station.ObjectsList[box.station.CountObj - 1], pos, transform.rotation);
                newObj.transform.parent = plr.transform;
                if (Count == 0) { ObjType = newObj.GetComponent<Object>().Type; }
                IngredientsCollection.Add(newObj);
                LustPosY += 0.1f;
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
                drop.AddObj(IngredientsCollection[0]);
                Destroy(IngredientsCollection[Count - 1]);
                IngredientsCollection.RemoveAt(Count - 1);
                LustPosY -= 0.1f;
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
                LustPosY -= 0.1f;
                Count--;
                if (Count == 0) { ObjType = ""; }
            }
        }
    }

    private IEnumerator DroppingObjectProc()
    {
        DropObject();
        yield return new WaitForSeconds(0.2f);
        if (drop != null || Bin != null)
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
            box = other.gameObject.GetComponent<TakeStation>();
            StartCoroutine(AddingObjectProc());
        }
        if (other.gameObject.GetComponent<DropStation>() != null)
        {
            drop = other.gameObject.GetComponent<DropStation>();
            StartCoroutine(DroppingObjectProc());
        }
        if(other.gameObject.tag == "Trashbin") 
        {
            Bin = other.gameObject;
            Debug.Log("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            StartCoroutine(DroppingObjectProc());
        }
    }



    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<TakeStation>() != null)
        {
            box = null;
            StopAllCoroutines();
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
    }
}