using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int speed, capacity, profit;
    public PlayerData(UpgreadeRoom meneger)
    {
        speed = meneger.Get("s");
        capacity = meneger.Get("c");
        profit = meneger.Get("p");
    }

}