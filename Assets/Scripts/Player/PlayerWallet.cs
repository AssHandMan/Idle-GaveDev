using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public enum TypeAccount { Free, Manager, SEO}

public class PlayerWallet : MonoBehaviour
{
    public Text Mtext, Dtext, Stext, Mtext2, Dtext2;
    public int Money, Diamonds, Skins;
    public float MoneyMultiplier;
    public TextMeshProUGUI EventText;
    private float MoneyForEvent;
    public GameObject MoneyEffectSpr;
    public Canvas canvas;
    private bool inProgress, vibrations;
    public TypeAccount typeAccount;

    void Start()
    {
        TextForm(Money, Mtext);
        TextForm(Diamonds, Dtext);
        TextForm(Skins, Stext);
        TextForm(Money, Mtext2);
        TextForm(Diamonds, Dtext2);
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void AddMoneyEvent()
    {   
        if(inProgress)
        {
            ChangeCount(MoneyForEvent, "Money");
            StopAllCoroutines();
        }
        MoneyForEvent = UnityEngine.Random.Range(30, 50);
        EventText.text = "$0";  
        EventText.gameObject.SetActive(true);
        StartCoroutine(AddingMoneyEvent());
        inProgress = true;
    }

    private IEnumerator AddingMoneyEvent()
    {
        for (int i = 0; i <= MoneyForEvent; i++)
        {
            yield return new WaitForSeconds(0.03f);
            EventText.text = "$" + i;
        }
        yield return new WaitForSeconds(1.5f);
        
        int CountMoneyImage = ((int)MoneyForEvent) / 5;
        for (int i = 0; i <= CountMoneyImage; i++)
        {
            yield return new WaitForSeconds(0.1f);
            Instantiate(MoneyEffectSpr, canvas.transform, false);
        }
        ChangeCount(MoneyForEvent, "Money");
        
        EventText.gameObject.SetActive(false);
        inProgress = false;
    }


    public void ChangeCount(float count, string type)
    {
        if(type == "Money")
        {
            if (count > 0) { Money += Convert.ToInt32(count * MoneyMultiplier); }
            else { Money += Convert.ToInt32(count); }
            TextForm(Money, Mtext);
            TextForm(Money, Mtext2);
            Vibrate();
        }
        else if (type == "Diamonds")
        {
            Diamonds += Convert.ToInt32(count);
            TextForm(Diamonds, Dtext);
            TextForm(Diamonds, Dtext2);
        }
        else
        {
            Skins += Convert.ToInt32(count);
            TextForm(Skins, Stext);
        }
    }
    public void SetVibrations(bool vibr)
    {
        vibrations = vibr;
    }

    private void Vibrate()
    {
        if (vibrations)
        {
            Handheld.Vibrate();
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
