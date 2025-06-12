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
    public GameObject Character, menu, ButtonsToHide, ButtonMain;
    public GameObject[] characters;
    public GameObject[] Player;
    public bool[] isBuy;
    public string[] priceList;
    public int SkinPos, SkinNow;
    public Animator animCurtain;
    public Text ButtonText;
    public Sprite[] ButtonsSpr;
    public Sprite[] MonyeType;
    public Image TypeMoneySkin;
    public int[] SoftHard;
    public GameObject[] Locks, SkinNowSpr;
    private PlayerWallet wallet;
    private bool CanChange;
    [SerializeField] private UpgreadeSceneMeneger upgreadeRoom;

    //Firebase TEST EVENT
    private FireBaseIniti fireBase_obj;

    void Start()
    {
        CanChange = true;
        ButtonText.text = "Take It";
        wallet = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerWallet>();

        //Firebase TEST EVENT
        fireBase_obj = Object.FindFirstObjectByType<FireBaseIniti>();
    }

    void Update()
    {
        Character.transform.Rotate(0, 20f*Time.deltaTime, 0);
    }
    public void ChangeSkinEffect(int pos)
    {
        if (CanChange)
        {
           StartCoroutine(CurtainTimeChange(pos));
        }
    }

    public void EquipeBuy()
    {
        if (!isBuy[SkinPos])
        {
            if (wallet.Skins >= Convert.ToInt32(priceList[SkinPos]) && SoftHard[SkinPos] == 0)
            {
                wallet.ChangeCount(-float.Parse(priceList[SkinPos]), "Skin");
                isBuy[SkinPos] = true;
                ButtonMain.GetComponent<Image>().sprite = ButtonsSpr[0];
                ButtonText.text = "Take It";
                TypeMoneySkin.sprite = MonyeType[2];
                Locks[SkinPos].SetActive(false);
                //Firebase TEST EVENT
                fireBase_obj.LogTestEvent2();
            }
            else if (wallet.Diamonds >= Convert.ToInt32(priceList[SkinPos]) && SoftHard[SkinPos] == 1)
            {
                wallet.ChangeCount(-float.Parse(priceList[SkinPos]), "Diamonds");
                isBuy[SkinPos] = true;
                ButtonMain.GetComponent<Image>().sprite = ButtonsSpr[0];
                ButtonText.text = "Take It";
                TypeMoneySkin.sprite = MonyeType[2];
                Locks[SkinPos].SetActive(false);

                //Firebase TEST EVENT
                fireBase_obj.LogTestEvent2();
            }
        }
        else
        {
            if(SkinPos != SkinNow)
            {
                ButtonMain.GetComponent<Image>().sprite = ButtonsSpr[0];
                ButtonText.text = "Take It";
                SkinNow = SkinPos;
                for(int i = 0; i < Player.Length; i++)
                {
                    if (i != SkinNow) { Player[i].SetActive(false); }
                    else { Player[i].SetActive(true); }
                }
                for (int i = 0; i < SkinNowSpr.Length; i++)
                {
                    if (i != SkinNow) { SkinNowSpr[i].SetActive(false); }
                    else { SkinNowSpr[i].SetActive(true); }
                }
            }
        }
    }

    public IEnumerator CurtainTimeChange(int newPos)
    {
        CanChange = false;
        animCurtain.SetTrigger("Move");
        yield return new WaitForSeconds(0.2f);
        characters[SkinPos].SetActive(false);
        SkinPos = newPos;
        characters[SkinPos].SetActive(true);
        Character.transform.eulerAngles = new Vector3(0, 70, 0);
        CanChange = true;

        if (!isBuy[SkinPos]) 
        { 
            ButtonText.text = priceList[SkinPos];
            if (SoftHard[SkinPos] == 0)
            {
                ButtonMain.GetComponent<Image>().sprite = ButtonsSpr[1];
                TypeMoneySkin.sprite = MonyeType[0];
            }
            else
            {
                ButtonMain.GetComponent<Image>().sprite = ButtonsSpr[2];
                TypeMoneySkin.sprite = MonyeType[1];
            }
            
        }
        else
        {
            ButtonMain.GetComponent<Image>().sprite = ButtonsSpr[0];
            TypeMoneySkin.sprite = MonyeType[2];
            ButtonText.text = "Take It";
        }
    }

    public void CloseMenu()
    {
        menu.SetActive(false);
        ButtonsToHide.SetActive(true);
        cam.SetPlayMood();
    }

    public void OpenMenu()
    {
        if (upgreadeRoom.LustPlace >= 34)
        {
            menu.SetActive(true);
            ButtonsToHide.SetActive(false);
            cam.SetSkinMood();
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerMovement>() != null)
        {
            OpenMenu();
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