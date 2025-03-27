using UnityEngine;
using UnityEngine.UI;

public class UpgreadeSceneMeneger : MonoBehaviour
{
    public BuySpace[] Spaces;
    public Text CountLvl;
    public int lvl, LustPlace;
    void Start()
    {
        CountLvl.text = lvl.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.W))
        {
            BuyAll();
        }   
    }

    public void BuyAll()
    {
        for (int i = 0; i < Spaces.Length; i++)
        {
            Spaces[i].Buy(true);
        }
    }

    public void LevelUp(BuySpace space)
    {
        for(int i = LustPlace; i < Spaces.Length; i++)
        {
            if (Spaces[i] == space)
            {
                if (i + 1 < Spaces.Length ) { Spaces[i + 1].gameObject.SetActive(true); }
                LustPlace = i;
                break;
            }
        }
        
        lvl++;
        CountLvl.text = lvl.ToString();
    }
}
