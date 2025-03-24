using UnityEngine;
using static System.Collections.Specialized.BitVector32;

public class point : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public int _countMoney;
    public float _deltaY;
    public float _nextY;
    public GameObject _moneyZone;

    public void Add(GameObject model)
    {
        _moneyZone.GetComponent<MoneyZoneController>()._cash3D.Add(Instantiate(model, this.transform.position, model.transform.rotation));        
    }
}
