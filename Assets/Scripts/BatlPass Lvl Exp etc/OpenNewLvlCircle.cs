using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class OpenNewLvlCircle : MonoBehaviour
{
    [SerializeField] private GameObject[] Icons;
    [SerializeField] private Text txt;
    [SerializeField] private GameObject MapBtn, ShopBtn;
    [SerializeField] private CustomersMeneger meneger;
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
    private string[] typesRus = {
    "ÕŒ¬€…\n“Œ¬¿–",
    "ÕŒ¬¿ﬂ\n«ŒÕ¿",
    "Õ¿√–¿ƒ¿",
    "—œ≈÷»¿À‹Õ€…\n«¿ ¿«",
    "—Œ¡€“»ﬂ",
    " ¿–“¿",
    "Ã¿√¿«»Õ",
    "ÕŒ¬€…\nÃ¿√¿«»Õ"
    };
    void Start()
    {

    }

    // New Product, Unlock Zone, Rewards for levels, spehial order, Disturbing Customers, map, Shop, New shop

    public void Open(string s)
    {
        if (LocalizationSettings.SelectedLocale == LocalizationSettings.AvailableLocales.Locales[0])
        {
            if (s == "New Product" || s == "new Product")
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
                meneger.DeliveryStart();
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
                MapBtn.SetActive(true);
            }
            else if (s == "Shop")
            {
                Icons[6].SetActive(true);
                txt.text = types[6];
                ShopBtn.SetActive(true);
            }
            else if (s == "New shop")
            {
                Icons[7].SetActive(true);
                txt.text = types[7];
            }
        }
        else
        {
            if (s == "New Product" || s == "new Product")
            {
                Icons[0].SetActive(true);
                txt.text = typesRus[0];
            }
            else if (s == "Unlock Zone")
            {
                Icons[1].SetActive(true);
                txt.text = typesRus[1];
            }
            else if (s == "Rewards for levels")
            {
                Icons[2].SetActive(true);
                txt.text = typesRus[2];
            }
            else if (s == "spehial order")
            {
                Icons[3].SetActive(true);
                txt.text = typesRus[3];
                meneger.DeliveryStart();
            }
            else if (s == "Disturbing Customers")
            {
                Icons[4].SetActive(true);
                txt.text = typesRus[4];
            }
            else if (s == "map")
            {
                Icons[5].SetActive(true);
                txt.text = typesRus[5];
                MapBtn.SetActive(true);
            }
            else if (s == "Shop")
            {
                Icons[6].SetActive(true);
                txt.text = typesRus[6];
                ShopBtn.SetActive(true);
            }
            else if (s == "New shop")
            {
                Icons[7].SetActive(true);
                txt.text = typesRus[7];
            }
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
