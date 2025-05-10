using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ArrowPosController : MonoBehaviour
{
    public UpgreadeSceneMeneger sceneMeneger;
    public GameObject[] stations;
    public bool inStationPosesProgress;
    public int posLvl;
    public List<Vector3> posStation;
    void Start()
    {
        for (int i = 0; i < stations.Length; i++)
        {
            Vector3 globalPosition = stations[i].transform.parent.TransformPoint(stations[i].transform.localPosition);
            globalPosition = new Vector3 (globalPosition.x, transform.position.y, globalPosition.z);
            posStation.Add(globalPosition);
        }
    }


    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("TheFirstShop");
        }
    }

    public void MoveNextToBuySpace()
    {
        if(!inStationPosesProgress)
        {
            if (sceneMeneger.lvl == 31)
            {
                posLvl = 12;
                inStationPosesProgress = true;
                MoveNextToStation();
            }
            else
            {
                if (sceneMeneger.LustPlace + 1 < sceneMeneger.NewSpaces.Length && sceneMeneger.LustPlace + 1 < sceneMeneger.lPos.Count)
                {
                    Vector3 globalPosition = sceneMeneger.NewSpaces[sceneMeneger.LustPlace + 1].transform.parent.TransformPoint(sceneMeneger.lPos[sceneMeneger.LustPlace + 1]);
                    transform.position = new Vector3(globalPosition.x, transform.position.y, globalPosition.z);
                    if (sceneMeneger.lvl == 3 || sceneMeneger.lvl == 11)
                    {
                        inStationPosesProgress = true;
                        MoveNextToStation();
                    }
                }
            }
        }
    }
    public void MoveNextToStation()
    {
        if (inStationPosesProgress)
        {
            if (posLvl == 11 || posLvl == 16)
            {
                inStationPosesProgress = false;
                MoveNextToBuySpace();
                posLvl++;
            }
            else
            {
                transform.position = posStation[posLvl];
                posLvl++;
            }
        }
    }
}
