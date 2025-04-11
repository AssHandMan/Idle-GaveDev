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
    public GameObject _imgCheckLvl;
    public GameObject _imgCheckGift;
    public GameObject _imgCheckFree;
    public GameObject _imgCheckMan;
    public GameObject _imgCheckSEO;
    private PlayerWallet wallet;
    private Exp_Controller exp_menu;

    public void Start()
    {
        wallet = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerWallet>();
        exp_menu = GameObject.FindGameObjectWithTag("ExpController").GetComponent<Exp_Controller>();  
        CheckPlayer();
    }
    public void Check(string text) 
    {
        _imgCheckLvl.SetActive(true);
        _imgCheckGift.SetActive(true);
        GetCash(text);
        CheckPlayer();
    }
    private void GetCash(string ars)
    {
        string[] temp = ars.Split(' ');
        //Debug.Log($"{temp[0]} {temp[1]} темп");
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
    private void CheckPlayer()
    {
        switch (wallet.typeAccount)
        {
            case TypeAccount.SEO:
                {
                    _imgCheckSEO.SetActive(false);
                    _imgCheckMan.SetActive(false);
                    _imgCheckFree.SetActive(false);
                    break;
                }
            case TypeAccount.Manager:
                {
                    _imgCheckMan.SetActive(false);
                    _imgCheckFree.SetActive(false);
                    break;
                }
        }
    }
    public void ClickFree()
    {
        if (_imgCheckFree.GetComponent<Image>().color != Color.green && exp_menu._playerLVL >= int.Parse(_Level.text) )
        {
            GetCash(_Free.text);
            _imgCheckFree.SetActive(true);
            _imgCheckFree.GetComponent<Image>().color = Color.green;
        }
    }
    public void ClickManager()
    {
        if ((wallet.typeAccount == TypeAccount.Manager || wallet.typeAccount == TypeAccount.SEO) &&
            _imgCheckMan.GetComponent<Image>().color != Color.green && exp_menu._playerLVL >= int.Parse(_Level.text)) 
        {
            GetCash(_Man.text);
            _imgCheckMan.SetActive(true);
            _imgCheckMan.GetComponent<Image>().color = Color.green;
        }
    }
    public void ClickSEO()
    {
        if (wallet.typeAccount == TypeAccount.SEO && _imgCheckSEO.GetComponent<Image>().color != Color.green &&
            exp_menu._playerLVL >= int.Parse(_Level.text))
        {
            GetCash(_SEO.text);
            _imgCheckSEO.SetActive(true);
            _imgCheckSEO.GetComponent<Image>().color = Color.green;
        }
    }

}
