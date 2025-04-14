using Firebase.Analytics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class SkinsRoom : MonoBehaviour
{
    public CameraController cam;
    public GameObject Character, menu;
    public GameObject[] characters;
    public GameObject[] Player;
    public bool[] isBuy;
    public string[] priceList;
    public int SkinPos, SkinNow;
    public Animator animCurtain;
    public Text ButtonText;

    private PlayerWallet wallet;
    private bool CanChange;

    //Firebase TEST EVENT
    private FireBaseIniti fireBase_obj;

    void Start()
    {
        CanChange = true;
        ButtonText.text = "Equipped";
        wallet = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerWallet>();

        //Firebase TEST EVENT
        fireBase_obj = Object.FindFirstObjectByType<FireBaseIniti>();
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
            if (wallet.Money >= Convert.ToInt32(priceList[SkinPos]))
            {
                wallet.ChangeCount(-float.Parse(priceList[SkinPos]), "Skin");
                isBuy[SkinPos] = true;
                ButtonText.text = "Equipe";

                //Firebase TEST EVENT
                fireBase_obj.LogTestEvent2();
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
            Character.transform.eulerAngles = new Vector3(0, 70, 0);
        }
        else
        {
            characters[SkinPos].SetActive(false);
            SkinPos--;
            characters[SkinPos].SetActive(true);
            Character.transform.eulerAngles = new Vector3(0, 70, 0);
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
        cam.SetPlayMood();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerMovement>() != null)
        {
            menu.SetActive(true);
            cam.SetSkinMood();
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