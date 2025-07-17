using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LvlObject : MonoBehaviour
{
    public Text  _Level;
    public Text _Gift;
    public Text _Free;
    public Text _Man;
    public Text _SEO;
    public GameObject _GiftMain;
    public GameObject _FreeMain;
    public GameObject _ManMain;
    public GameObject _SEOMain;
    public GameObject _imgCheckLvl;
    public GameObject _imgCheckGift;
    public GameObject _imgCheckFree;
    public GameObject _imgCheckMan;
    public GameObject _imgCheckSEO;
    public GameObject _imgGift;
    public GameObject _imgFree;
    public GameObject _imgMan;
    public GameObject _imgSEO;
    private PlayerWallet wallet;
    private Exp_Controller exp_menu;
    public  Button _btn_free;
    public Button _btn_man;
    public Button _btn_seo;


    public void Start()
    {
        wallet = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerWallet>();
        exp_menu = GameObject.FindGameObjectWithTag("ExpController").GetComponent<Exp_Controller>();  
        CheckPlayer();
        CheckForBTN();
        if ( _Level.text == "1") _imgCheckLvl.SetActive(true);
    }
    public void CheckForBTN()
    {
        if (PlayerPrefs.GetInt(_Level.text + " Free", 1) == 99)
            _btn_free.gameObject.SetActive(false);
        if (PlayerPrefs.GetInt(_Level.text + " Man", 1) == 99)
            _btn_man.gameObject.SetActive(false);
        if (PlayerPrefs.GetInt(_Level.text + " SEO", 1) == 99)
            _btn_seo.gameObject.SetActive(false);
    }
    public void Check(string text) 
    {
        _imgCheckLvl.SetActive(true);
        //_imgCheckGift.SetActive(true);
        Debug.Log("checl");
        GetCash(text);
        CheckPlayer();
    }
    private void GetCash(string ars)
    {
        string[] temp = ars.Split(' ');
        if (temp[0] != "" && temp[0] != "-")
        {
            Debug.Log($"{temp[0]} {temp[1]} темп");
            if (temp[1] == "b")
            {
                temp[1] = "Money";
            }
            else if (temp[1] == "k")
            {
                temp[1] = "Diamonds";
            }
            else
            {
                temp[1] = "Tokens";
            }
            wallet.ChangeCount(float.Parse(temp[0]), temp[1]);
        }
    }
    public void CheckPlayer()
    {
        switch (wallet.typeAccount)
        {
            case TypeAccount.SEO:
                {
                    _imgCheckSEO.SetActive(false);
                    _btn_seo.gameObject.SetActive(true);
                    _imgCheckMan.SetActive(false);
                    _btn_man.gameObject.SetActive(true);
                    _imgCheckFree.SetActive(false);
                    break;
                }
            case TypeAccount.Manager:
                {
                    _imgCheckMan.SetActive(false);
                    _imgCheckFree.SetActive(false);
                    _btn_man.gameObject.SetActive(true);
                    break;
                }
        }

        if (PlayerPrefs.GetInt("PlayerLVL", -1) >= int.Parse(_Level.text))
            _imgCheckLvl.SetActive(true);
    }
    public void ClickFree()
    {
        if (exp_menu._playerLVL >= int.Parse(_Level.text) )
        {
            GetCash(_Free.text);
            _btn_free.gameObject.SetActive(false);
        }
        PlayerPrefs.SetInt(_Level.text+" Free",99);
        PlayerPrefs.Save();
    }
    public void ClickManager()
    {
        if ((wallet.typeAccount == TypeAccount.Manager || wallet.typeAccount == TypeAccount.SEO) &&
            _imgCheckMan.GetComponent<Image>().color != Color.green && exp_menu._playerLVL >= int.Parse(_Level.text)) 
        {
            GetCash(_Man.text);
            _btn_man.gameObject.SetActive(false);
        }
        PlayerPrefs.SetInt(_Level.text + " Man", 99);
        PlayerPrefs.Save();
    }
    public void ClickSEO()
    {
        if (wallet.typeAccount == TypeAccount.SEO && _imgCheckSEO.GetComponent<Image>().color != Color.green &&
            exp_menu._playerLVL >= int.Parse(_Level.text))
        {
            GetCash(_SEO.text);
            _btn_seo.gameObject.SetActive(false);
        }
        PlayerPrefs.SetInt(_Level.text + " SEO", 99);
        PlayerPrefs.Save();
    }

}
