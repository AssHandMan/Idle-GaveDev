using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgreadeSceneMeneger : MonoBehaviour
{
    public ArrowPosController arrow;
    public BuySpace[] NewSpaces;

    public List<int> PriceList;
    public List<Vector3> lPos;
    public List<GameObject> stations, parents;

    public Text CountLvl;
    public int lvl, LustPlace;
    void Start()
    {
        CountLvl.text = lvl.ToString();

        for (int i = 0; i < NewSpaces.Length; i++)
        {
            NewSpaces[i].price = PriceList[i];
            NewSpaces[i].station = stations[i];
            NewSpaces[i].transform.parent = parents[i].transform;
            NewSpaces[i].transform.localPosition = lPos[i];
        }
    }

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
        NewSpaces[LustPlace].Buy(true);
        LustPlace++;
        NewSpaces[LustPlace].gameObject.SetActive(true);
        lvl++;
        CountLvl.text = lvl.ToString();

    }

    public void BuyAll()
    {
        for (int i = 0; i < NewSpaces.Length; i++)
        {
            NewSpaces[i].Buy(true);
        }
    }

    public void LevelUp(BuySpace space)
    {
        for(int i = LustPlace; i < NewSpaces.Length; i++)
        {
            if (NewSpaces[i] == space)
            {
                if (i + 1 < NewSpaces.Length ) { NewSpaces[i + 1].gameObject.SetActive(true); }
                LustPlace = i;
                arrow.MoveNextToBuySpace();
                break;
            }
        }
        
        lvl++;
        CountLvl.text = lvl.ToString();
    }
}
