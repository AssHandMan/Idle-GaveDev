using System;
using Unity.VisualScripting;
using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEngine;
using UnityEngine.UI;

public class shopButton : MonoBehaviour
{
    [SerializeField] private int _time, _cash, _maxSize = 0;
    [SerializeField] string _numShop = "0";
    [SerializeField] private NetworkTimeChecker _timeChecker;
    [SerializeField] private Text _btnText;
    [SerializeField] private GameObject _btn;
    [SerializeField] private Shop _shop;
    [SerializeField] private GameObject _shopInfo;
    [SerializeField] private Text _shopInfo_text;
    [SerializeField] private string _lvlForActivated;

    private int X;
    public void Start()
    {
        _timeChecker.WriteTimeEnterMap();
        DateTime LastGift = DateTime.Now;
        _timeChecker.GetNetworkTime ((nowTime) => {
            LastGift = DateTime.Parse(PlayerPrefs.GetString("LastGiveTime" + _numShop, nowTime.ToString()));
        });
        
        DateTime EnterMap = DateTime.Parse(PlayerPrefs.GetString("TimeEnterMap", "0"));

        Debug.Log("Last give "+_numShop +": "+ LastGift.ToString() + " enter map: " + EnterMap.ToString());
        Debug.Log("Wallet "+_numShop +": "+ PlayerPrefs.GetInt("Wallet" + _numShop, 0).ToString());

        int dif = (int)((EnterMap - LastGift).TotalSeconds);

        //Debug.Log(dif);

        X = (dif / _time) * _cash;
        if (X > _maxSize)
            X = _maxSize;

       
        PlayerPrefs.SetInt("Wallet" + _numShop, X);
        PlayerPrefs.Save();



        _btnText.text = PlayerPrefs.GetInt("Wallet" + _numShop, 1).ToString();
        Debug.Log("shop "+_numShop+": " + PlayerPrefs.GetInt("Wallet" + _numShop, 1).ToString());
        if (_btnText.text != "" && _btnText.text != "0" && _shop._isActive)
            _btn.SetActive(true);
    }
    public void OnClick()
    {
        PlayerPrefs.SetInt("Agree" + _numShop, 1);
        if (PlayerPrefs.GetInt("Wallet"+_numShop,0) > 0)
        {
            UpdatePrefs();
        }
        _btn.SetActive(false);
        PlayerPrefs.Save();

    }
    public void OnInfo()
    {
        _shopInfo_text.supportRichText = true;
        _shopInfo_text.text += $"<color=red>{_lvlForActivated}</color> Б \n<color=red>опедшдсыел люцюгхме</color>";
        _shopInfo.SetActive(true);
    }
    public void CloseInfo()
    {
        _shopInfo_text.text = "мнбши люцюгхм нрйпнеряъ,\r\nйнцдю бш днярхцмере\r\nгб╗гдмнцн спнбмъ ";
        _shopInfo.SetActive(false);
    }
    private void UpdatePrefs()
    {
        
        _timeChecker.WriteTimeLastGiveTime(_numShop);
    }
    
}
