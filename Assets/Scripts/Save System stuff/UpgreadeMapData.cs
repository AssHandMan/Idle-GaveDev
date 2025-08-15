using UnityEngine;


[System.Serializable]
public class UpgreadeMapData
{
    public int LustPlace;
    public UpgreadeMapData(UpgreadeSceneMeneger meneger)
    {
        LustPlace = meneger.LustPlace;
    }

}
