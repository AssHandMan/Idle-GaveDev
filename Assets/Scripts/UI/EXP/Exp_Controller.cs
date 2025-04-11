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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Parse()
    {
        string[] lines = _csvData.text.Split('\n');

        //изменить когда будет норм таблица
        for (int i = 2; i < lines.Length; i++)
        {
            GameObject panelLevel = _panelLevel;
            string line = lines[i].Trim();
            if (string.IsNullOrEmpty(line))
                continue;
            string[] fields = line.Split(',');
            CreatePanel(fields, panelLevel);
            GameObject newObj = Instantiate(_panelLevel, _ScrollView.transform);
            _Lvls.Add(newObj);
            
        }
    }
    private void CreatePanel(string[] lines, GameObject panel)
    {
        LvlObject temp = panel.GetComponent<LvlObject>();
        temp._Level.text = lines[0];
        _sliderLVLs.Add(lines[1]);
        temp._Gift.text = lines[2];
        temp._Free.text = lines[3];
        temp._Man.text = lines[4];
        temp._SEO.text = lines[5];
        _gifts.Add(panel.GetComponent<LvlObject>());
    }
    private void NewLvl()
    {
        _btn.SetActive(false);
        _newLVL.SetActive(true);
        _text_NewLVL.text = _Lvls[_playerLVL].GetComponent<LvlObject>()._Gift.text;
        _slider.maxValue = float.Parse(_sliderLVLs[_playerLVL+1]);
        _Lvls[_playerLVL].GetComponent<LvlObject>().Check(_Lvls[_playerLVL].GetComponent<LvlObject>()._Gift.text);
        _playerLVL++;

    }
    public void GetEXP(int exp)
    {
        _playerEXP += exp;
        if(_playerEXP >= _slider.maxValue)
            NewLvl();
        _slider.value = _playerEXP;
        _LabelLVL.text = _playerLVL.ToString();
    }
    public void CloseLVL()
    {
        _btn.SetActive(true);
        _newLVL.SetActive(false);
    }
}
