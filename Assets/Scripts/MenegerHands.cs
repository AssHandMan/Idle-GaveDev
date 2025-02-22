using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static System.Collections.Specialized.BitVector32;

public class MenegerHands : MonoBehaviour
{
    public Meneger meneger;
    public List<GameObject> IngredientsCollection;
    public int Count;
    public float LustPosY;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<ArcadeMachine>() != null && meneger.target == other.GetComponent<ArcadeMachine>().CustomerPos)
        {
            ArcadeMachine a = other.GetComponent<ArcadeMachine>();
            Vector3 pos = new Vector3(transform.position.x, LustPosY, transform.position.z);
            GameObject newObj = Instantiate(a.station.ObjectsList[a.station.CountObj - 1], pos, transform.rotation);
            newObj.transform.parent = meneger.gameObject.transform;
            IngredientsCollection.Add(newObj);
            LustPosY += 0.1f;
            Count++;
            other.GetComponent<TakeStation>().DeleteObj();

            meneger.RemoveFirst();
            gameObject.SetActive(false);

        }
        
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<Trashbin>() != null)
        {
            for (int i = 0; i < IngredientsCollection.Count; i++)
            {
                Destroy(IngredientsCollection[i]);
                IngredientsCollection.RemoveAt(i);
                Count--;
                LustPosY -= 0.1f;
            }
            if (Count == 0)
            {
                meneger.inProgress = false;
                meneger.StartCoroutine(meneger.DefineTarget());
                gameObject.SetActive(false);
            }
        }
    }
}
