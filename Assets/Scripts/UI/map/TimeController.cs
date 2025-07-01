using System;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    [SerializeField] private string _NameStartFunc;
    [SerializeField] private PlayerWallet _PlayerWallet;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        NetworkTimeChecker NTC = GetComponent<NetworkTimeChecker>();
        switch (_NameStartFunc)
        {
            case "WriteTimeLastGiveTime":
                //NTC.WriteTimeLastGiveTime();
                break;
            case "WriteTimeEnterMap":
                NTC.WriteTimeEnterMap();
                break;
        }
    }
}
