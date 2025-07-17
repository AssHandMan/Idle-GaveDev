using NUnit.Framework;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.UI;

public class Exp_Controller : MonoBehaviour
{
    public GameObject _panelLevel;
    public List<GameObject> _Lvls = new List<GameObject>();
    public GameObject _ScrollView;
    public TextAsset _csvData;
    [SerializeField] private Slider _slider;
    [SerializeField] private List<string> _sliderLVLs = new List<string>();
    [SerializeField] public int _playerLVL = 1;
    [SerializeField] private int _playerEXP = 0;
    [SerializeField] private Text _text_NewLVL;
    [SerializeField] private List<LvlObject> _gifts = new List<LvlObject>();
    [SerializeField] private GameObject _newLVL;
    [SerializeField] private GameObject _NEWbtn;
    [SerializeField] private GameObject _btn;
    [SerializeField] private Text _LabelLVL;
    [SerializeField] private Sprite _money;
    [SerializeField] private Sprite _skins;
    [SerializeField] private Sprite _diamonds;
    [SerializeField] private Image _voidImg;
    [SerializeField] private Text _sliderText;
    [SerializeField] private NetworkTimeChecker _timeChecker;

    [SerializeField] private List<Sprite> _icons = new List<Sprite>();
    [SerializeField] private Dictionary<string, Sprite> _Slovar = new Dictionary<string, Sprite>();
    [SerializeField] private PlayerWallet _playerWallet;
    [SerializeField] private GameObject _btnMan;
    [SerializeField] private GameObject _btnSEO;
    [SerializeField] private List<string> _unLock = new List<string>();
    [SerializeField] private List<GameObject> _unLockCelltext = new List<GameObject>();
    [SerializeField] private List<GameObject> _unLockCellImg = new List<GameObject>();
    [SerializeField] private List<Sprite> _unlockSprite = new List<Sprite>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Parse()
    {
        string[] lines = _csvData.text.Split('\n');

        for (int i = 2; i < lines.Length; i++)
        {
            GameObject panelLevel = _panelLevel;
            string line = lines[i].Trim();
            if (string.IsNullOrEmpty(line))
                continue;
            string[] fields = line.Split(',');
            CreatePanel(fields, panelLevel);
            GameObject newObj = Instantiate(_panelLevel, _ScrollView.transform);
            if (newObj.GetComponent<LvlObject>()._Gift.text == "")
                newObj.GetComponent<LvlObject>()._GiftMain.gameObject.SetActive(false);
            _Lvls.Add(newObj);

        }
    }
    private void CreatePanel(string[] lines, GameObject panel)
    {
        LvlObject temp = panel.GetComponent<LvlObject>();
        temp._Level.text = lines[0];
        _sliderLVLs.Add(lines[1]);

        //Debug.Log(lines[2]);

        temp._Gift.text = lines[2];
        temp._imgGift.GetComponent<Image>().sprite = SelectImgIcon(lines[2]);


        temp._Free.text = lines[3];
        temp._imgFree.GetComponent<Image>().sprite = SelectImgIcon(lines[3]);


        temp._Man.text = lines[4];
        temp._imgMan.GetComponent<Image>().sprite = SelectImgIcon(lines[4]);


        temp._SEO.text = lines[5];
        temp._imgSEO.GetComponent<Image>().sprite = SelectImgIcon(lines[5]);

        _unLock.Add(lines[6]);
        _gifts.Add(panel.GetComponent<LvlObject>());

    }
    private Sprite SelectImgIcon(string str)
    {
        //Debug.Log(str);
        if (str != "-" && str != "")
        {
            //Debug.Log(str);
            string type = str.Split(" ")[1];
            if (type == "b")
            {
                return _money;
            }
            else if (type == "k")
            {
                return _diamonds;
            }
            else if (type == "t")
            {
                return _skins;
            }
            else
            {
                return null;
            }
        }

        return null;
    }
    private Sprite SelectImgUnlock(string str)
    {
            if (str.Contains("Unlock Zone"))
                return _unlockSprite[1];
            else if (str.Contains("New Product"))
                return _unlockSprite[2];
            else if (str.Contains("spehial order"))
                return _unlockSprite[3];
            else if (str.Contains("Disturbing Customers"))
                return _unlockSprite[4];
            else if (str.Contains("map"))
                return _unlockSprite[5];
            else if (str.Contains("New Shop"))
                return _unlockSprite[6];
            else return null;
    }

    private void NewLvl()
    {
        foreach (var i in _unLockCellImg)
        {
            i.SetActive(false);
        }
        _btn.SetActive(false);
        _newLVL.SetActive(true);
        _text_NewLVL.text = _Lvls[_playerLVL].GetComponent<LvlObject>()._Gift.text;
        _slider.maxValue = float.Parse(_sliderLVLs[_playerLVL + 1]);
        _Lvls[_playerLVL].GetComponent<LvlObject>().Check(_Lvls[_playerLVL].GetComponent<LvlObject>()._Gift.text);
        _playerLVL++;

        if (_unLock[_playerLVL] != "")
        {
            string[] temp = _unLock[_playerLVL].Split(",");
            Debug.Log(_unLock[_playerLVL]);
            switch (temp.Length)
            {
                case 1:
                    _unLockCellImg[1].gameObject.SetActive(true);
                    _unLockCellImg[1].gameObject.GetComponent<Image>().sprite = SelectImgUnlock(temp[0]);
                    _unLockCelltext[1].gameObject.GetComponent<Text>().text = temp[0];
                    break;
                case 2:
                    _unLockCellImg[0].gameObject.SetActive(true);
                    _unLockCellImg[0].gameObject.GetComponent<Image>().sprite = SelectImgUnlock(temp[0]);
                    _unLockCelltext[0].gameObject.GetComponent<Text>().text = temp[0];
                    _unLockCellImg[2].gameObject.SetActive(true);
                    _unLockCellImg[2].gameObject.GetComponent<Image>().sprite = SelectImgUnlock(temp[1]);
                    _unLockCelltext[2].gameObject.GetComponent<Text>().text = temp[1];
                    break;
                case 3:
                    _unLockCellImg[0].gameObject.SetActive(true);
                    _unLockCellImg[0].gameObject.GetComponent<Image>().sprite = SelectImgUnlock(temp[0]);
                    _unLockCelltext[0].gameObject.GetComponent<Text>().text = temp[0];
                    _unLockCellImg[1].gameObject.SetActive(true);
                    _unLockCellImg[1].gameObject.GetComponent<Image>().sprite = SelectImgUnlock(temp[1]);
                    _unLockCelltext[1].gameObject.GetComponent<Text>().text = temp[1];
                    _unLockCellImg[2].gameObject.SetActive(true);
                    _unLockCellImg[2].gameObject.GetComponent<Image>().sprite = SelectImgUnlock(temp[2]);
                    _unLockCelltext[2].gameObject.GetComponent<Text>().text = temp[2];
                    break;
            }
        }
        PlayerPrefs.SetFloat("SliderMaxValue", _slider.maxValue);
        PlayerPrefs.SetInt("PlayerLVL", _playerLVL);
        PlayerPrefs.Save();


    }
    public void GetEXP(int exp)
    {
        _playerEXP += exp;
        if (_playerEXP >= _slider.maxValue)
            NewLvl();
        _slider.value = _playerEXP;
        _LabelLVL.text = PlayerPrefs.GetInt("PlayerLVL", 1).ToString();
        PlayerPrefs.SetInt("PlayerEXP", _playerEXP);
        PlayerPrefs.Save();

        Debug.Log("Exp" + PlayerPrefs.GetInt("PlayerEXP", 0).ToString());
        Debug.Log("LVL" + PlayerPrefs.GetInt("PlayerLVL", 1).ToString());

        _sliderText.text = _slider.value.ToString() + "/" + _slider.maxValue.ToString();
    }
    public void CloseLVL()
    {
        _btn.SetActive(true);
        _newLVL.SetActive(false);
    }
    public void CheckLvL()
    {
        _playerLVL = PlayerPrefs.GetInt("PlayerLVL", 1);
        _playerEXP = PlayerPrefs.GetInt("PlayerEXP", 0);
        _slider.maxValue = PlayerPrefs.GetFloat("SliderMaxValue", 15);
        _slider.value = _playerEXP;
        _sliderText.text = _slider.value.ToString() + "/" + _slider.maxValue.ToString();
        switch (PlayerPrefs.GetString("TypeAccount", "Free"))
        {
            case "SEO":
                _playerWallet.typeAccount = TypeAccount.SEO;
                _btnMan.SetActive(false);
                _btnSEO.SetActive(false);
                break;
            case "Manager":
                _playerWallet.typeAccount = TypeAccount.Manager;
                _btnMan.SetActive(false);
                break;
            default:
                _playerWallet.typeAccount = TypeAccount.Free;
                break;
        }
    }
    public void ResetAllPrefs()
    {
        PlayerPrefs.DeleteAll();
        _timeChecker.WriteTimeLastGiveTime("0");
        _timeChecker.WriteTimeLastGiveTime("1");
        _timeChecker.WriteTimeLastGiveTime("2");

    }
    public void BuyAccountManager()
    {
        _playerWallet.typeAccount = TypeAccount.Manager;
        _btnMan.SetActive(false);
        PlayerPrefs.SetString("TypeAccount", "Manager");
        PlayerPrefs.Save();
        UpdatePanelLVL();
    }
    public void BuyAccountSEO()
    {
        _playerWallet.typeAccount = TypeAccount.SEO;
        _btnSEO.SetActive(false);
        PlayerPrefs.SetString("TypeAccount", "SEO");
        PlayerPrefs.Save();
        UpdatePanelLVL();

    }
    private void UpdatePanelLVL()
    {
        foreach (GameObject i in _Lvls)
        {
            i.GetComponent<LvlObject>().CheckPlayer();
        }
    }
}
