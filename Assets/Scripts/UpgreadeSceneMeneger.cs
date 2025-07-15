using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.AI.Navigation;

public class UpgreadeSceneMeneger : MonoBehaviour
{
    public ArrowPosController arrow;
    public NavMeshSurface surface;
    public BuySpace[] NewSpaces;

    private int[] PriceList = {
    25, 5, 50, 100, 50, 100, 40, 60, 100, 200,
    150, 50, 25, 230, 200, 500, 250, 120, 290, 330,
    370, 410, 450, 900, 900, 500, 550, 600, 650, 700,
    750, 1400, 100, 50, 50, 100, 700, 760, 820, 880,
    940, 1000, 1050, 2000, 1000, 230, 1080, 1120, 1160, 1210,
    1240, 1280, 1350, 2700, 1350, 1410, 1470, 1530, 1590, 100,
    1710, 1750, 1750, 1830, 1870, 1870, 1910, 1950, 1990, 2030,
    2090, 2150, 2150, 2180, 2210, 2240, 2270, 2300, 2330, 2380,
    2390, 2400, 2430, 2490, 2490, 2550, 2550
};
    public List<Vector3> lPos;
    public List<GameObject> stations;

    public Text CountLvl;
    public int lvl, LustPlace;
    [SerializeField] private ExpLvlController ExpController;
    [SerializeField] private CameraController cam;
    private BuySpace LustBuySpace;
    private int[] UpgreadeIndex = new int[] { 8, 17, 19, 20, 21, 22, 24, 26, 27, 37, 38, 39, 40, 41, 45, 46, 47, 48, 49, 50, 51, 55, 56, 58, 61, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85 };
    int[] IndexForCountNextLvl = { 0, 4, 8, 14, 22, 29, 41, 51, 60, 70, 84 };
    void Start()
    {
        CountLvl.text = lvl.ToString();

        for (int i = 0; i < NewSpaces.Length; i++)
        {
            NewSpaces[i].price = PriceList[i];
            NewSpaces[i].station = stations[i];
            NewSpaces[i].transform.localPosition = lPos[i];
        }
        LustBuySpace = NewSpaces[0];
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

    public void Destenition()
    {
        cam.SetBuySpacePos(LustBuySpace.gameObject);
    }

    public void BuyNext()
    {
        NewSpaces[LustPlace].Buy(true);
        LustPlace++;
        NewSpaces[LustPlace].gameObject.SetActive(true);
        lvl++;
        CountLvl.text = lvl.ToString();
        surface.BuildNavMesh();
    }

    public void BuyAll()
    {
        for (int i = 0; i < NewSpaces.Length; i++)
        {
            NewSpaces[i].Buy(true);
        }
        surface.BuildNavMesh();
        LustPlace = NewSpaces.Length;
    }

    public void LevelUp(BuySpace space)
    {
        for(int i = LustPlace; i < NewSpaces.Length; i++)
        {
            if (NewSpaces[i] == space)
            {
                if (i + 1 < NewSpaces.Length ) 
                { 
                    NewSpaces[i + 1].gameObject.SetActive(true);
                    LustBuySpace = NewSpaces[i + 1];
                }
                LustPlace = i;
                arrow.MoveNextToBuySpace();
                break;
            }
        }
        ExpController.InscreaseExP(space.station);
        lvl++;
        CountLvl.text = lvl.ToString();
        surface.BuildNavMesh();
    }

    public bool CheckIsUpgreade(BuySpace space)
    {
        int number = -999;
        bool res = false;
        for(int i = 0; i < NewSpaces.Length; i++)
        {
            if (NewSpaces[i] == space)
            {
                number = i;
                break;
            }
        }
        for (int i = 0; i<UpgreadeIndex.Length; i++)
        {
            if(number == UpgreadeIndex[i])
            {
                res = true;
            }
        }
        return res;
    }

    public int GetMaxLvl()
    {
        int a = IndexForCountNextLvl[ExpController.lvl];
        int n = 0;
        for (int i = 0; i <= a; i++)
        {
            if ((stations[i].GetComponent<ObjectShowHider>() != null && stations[i].name == "AddSpace0")
                || (stations[i].GetComponent<ObjectShowHider>() == null))
            {
                n++;
            }
        }
        return n;
    }
}
