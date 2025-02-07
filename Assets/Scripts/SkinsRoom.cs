using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class SkinsRoom : MonoBehaviour
{
    public GameObject Character, menu, MainCam, SkinCam;
    public GameObject[] characters;
    public GameObject[] Player;
    public bool[] isBuy;
    public string[] priceList;
    public int SkinPos, SkinNow;
    public Animator animCurtain;
    public Text ButtonText;

    private PlayerWallet wallet;
    private bool CanChange;

    void Start()
    {
        CanChange = true;
        ButtonText.text = "Equipped";
        wallet = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerWallet>();
    }

    // Update is called once per frame
    void Update()
    {
        Character.transform.Rotate(0, 20f*Time.deltaTime, 0);
    }

    public void Forward()
    {
        if(SkinPos < characters.Length-1 && CanChange) 
        {
            StartCoroutine(CurtainTimeChange(true));
        }
    }
    public void Back()
    {
        if (SkinPos > 0 && CanChange)
        {
           StartCoroutine(CurtainTimeChange(false));
        }
    }

    public void EquipeBuy()
    {
        if (!isBuy[SkinPos])
        {
            if (wallet.Money > Convert.ToInt32(priceList[SkinPos]))
            {
                wallet.ChangeCount(-float.Parse(priceList[SkinPos]), "Money");
                isBuy[SkinPos] = true;
                ButtonText.text = "Equipe";
            }
        }
        else
        {
            if(SkinPos != SkinNow)
            {
                ButtonText.text = "Equipped";
                SkinNow = SkinPos;
                for(int i = 0; i < Player.Length; i++)
                {
                    if (i != SkinNow) { Player[i].SetActive(false); }
                    else { Player[i].SetActive(true); }
                }
            }
        }
    }

    public IEnumerator CurtainTimeChange(bool isForward)
    {
        CanChange = false;
        animCurtain.SetTrigger("Move");
        yield return new WaitForSeconds(0.2f);
        if(isForward)
        {
            characters[SkinPos].SetActive(false);
            SkinPos++;
            characters[SkinPos].SetActive(true);
        }
        else
        {
            characters[SkinPos].SetActive(false);
            SkinPos--;
            characters[SkinPos].SetActive(true);
        }
        CanChange = true;

        if (!isBuy[SkinPos]) { ButtonText.text = priceList[SkinPos]; }
        else
        {
            if(SkinPos == SkinNow) { ButtonText.text = "Equipped"; }
            else { ButtonText.text = "Equipe"; }
        }
    }

    public void CloseMenu()
    {
        menu.SetActive(false);
        MainCam.SetActive(true);
        SkinCam.SetActive(false);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerMovement>() != null)
        {
            menu.SetActive(true);
            MainCam.SetActive(false);
            SkinCam.SetActive(true);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerMovement>() != null)
        {
            CloseMenu();
        }
    }
}
