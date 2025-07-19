using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class OpenNewLvlCircle : MonoBehaviour
{
    [SerializeField] private GameObject[] Icons;
    [SerializeField] private Text txt;
    private string[] types = {
    "NEW\nPRODUCT",
    "NEW\nZONE",
    "LEVEL\nREWARDS",
    "SPECIAL\nORDER",
    "DISTURBING\nCUSTOMERS",
    "MAP",
    "SHOP",
    "NEW\nSHOP"
    };

    void Start()
    {

    }

    // New Product, Unlock Zone, Rewards for levels, spehial order, Disturbing Customers, map, Shop, New shop

    public void Open(string s)
    {
        if (s == "New Product")
        {
            Icons[0].SetActive(true);
            txt.text = types[0];
        }
        else if (s == "Unlock Zone")
        {
            Icons[1].SetActive(true);
            txt.text = types[1];
        }
        else if (s == "Rewards for levels")
        {
            Icons[2].SetActive(true);
            txt.text = types[2];
        }
        else if (s == "spehial order")
        {
            Icons[3].SetActive(true);
            txt.text = types[3];
        }
        else if (s == "Disturbing Customers")
        {
            Icons[4].SetActive(true);
            txt.text = types[4];
        }
        else if (s == "map")
        {
            Icons[5].SetActive(true);
            txt.text = types[5];
        }
        else if (s == "Shop")
        {
            Icons[6].SetActive(true);
            txt.text = types[6];
        }
        else if (s == "New shop")
        {
            Icons[7].SetActive(true);
            txt.text = types[7];
        }
    }

    public void Cleare()
    {
        for (int i = 0; i < Icons.Length; i++)
        {
            Icons[i].SetActive(false);
        }
        txt.text = "";
    }
}
