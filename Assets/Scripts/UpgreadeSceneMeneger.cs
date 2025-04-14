using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgreadeSceneMeneger : MonoBehaviour
{
    public BuySpace[] SpacesNew;
    public List<GameObject> parents;
    public List<GameObject> stations;
    public List<int> PriceList;
    public List<Vector3> positions;
    public Text CountLvl;
    public int lvl, LustPlace;
    void Start()
    {
        CountLvl.text = lvl.ToString();

        for (int i = 0; i < SpacesNew.Length; i++)
        {
            SpacesNew[i].price = PriceList[i];
        }
        for (int i = 0; i < SpacesNew.Length; i++)
        {
            SpacesNew[i].station = stations[i];
            SpacesNew[i].price = PriceList[i];
            SpacesNew[i].transform.parent = parents[i].transform;
            SpacesNew[i].transform.localPosition = positions[i];
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.W))
        {
            BuyAll();
        }
        if (Input.GetKeyUp(KeyCode.Q))
        {
            BuyNext();
        }
    }

    public void BuyNext()
    {
        SpacesNew[LustPlace].Buy(true);
        LustPlace++;
        SpacesNew[LustPlace].gameObject.SetActive(true);
    }

    public void BuyAll()
    {
        for (int i = 0; i < SpacesNew.Length; i++)
        {
            SpacesNew[i].Buy(true);
        }
    }

    public void LevelUp(BuySpace space)
    {
        for(int i = LustPlace; i < SpacesNew.Length; i++)
        {
            if (SpacesNew[i] == space)
            {
                if (i + 1 < SpacesNew.Length ) { SpacesNew[i + 1].gameObject.SetActive(true); }
                LustPlace = i;
                break;
            }
        }
        
        lvl++;
        CountLvl.text = lvl.ToString();
    }
}
