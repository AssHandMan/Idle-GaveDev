using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum TypeAccount { Free, Manager, SEO}

public class PlayerWallet : MonoBehaviour
{
    public Text Mtext, Dtext, Stext;
    public int Money, Diamonds, Skins;
    public float MoneyMultiplier;
    public TypeAccount typeAccount;
    void Start()
    {
        TextForm(Money, Mtext);
        TextForm(Diamonds, Dtext);
        TextForm(Skins, Stext);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeCount(float count, string type)
    {
        if(type == "Money")
        {
            if (count > 0) { Money += Convert.ToInt32(count * MoneyMultiplier); }
            else { Money += Convert.ToInt32(count); }
            TextForm(Money, Mtext);
        }
        else if (type == "Diamonds")
        {
            Diamonds += Convert.ToInt32(count);
            TextForm(Diamonds, Dtext);
        }
        else
        {
            Skins += Convert.ToInt32(count);
            TextForm(Skins, Stext);
        }
    }

    public void TextForm(int Number, Text CountText)
    {
        if (Number < 1000) { CountText.text = Number.ToString(); }
        else if (Number >= 1000 && Number < 100000) { CountText.text = Convert.ToString(Number / 1000) + "." + Convert.ToString((Number % 1000 - Number % 10) / 10) + "k"; }
        else if (Number >= 100000 && Number < 1000000) { CountText.text = Convert.ToString(Number / 100000) + "." + Convert.ToString((Number % 100000 - Number % 100) / 1000) + "kk"; }
        else if (Number >= 1000000 && Number < 100000000) { CountText.text = Convert.ToString(Number / 1000000) + "." + Convert.ToString((Number % 1000000 - Number % 1000) / 10000) + "m"; }
        else if (Number >= 100000000 && Number < 1000000000) { CountText.text = Convert.ToString(Number / 100000000) + "." + Convert.ToString((Number % 100000000 - Number % 10000) / 1000000) + "mm"; }
        else if (Number >= 1000000000) { CountText.text = Convert.ToString(Number / 1000000000) + "." + Convert.ToString((Number % 1000000000 - Number % 100000) / 10000000) + "b"; }
    }
}
