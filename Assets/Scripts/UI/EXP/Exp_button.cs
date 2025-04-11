using UnityEngine;
using UnityEngine.UI;

public class Exp_button : MonoBehaviour
{
    public GameObject _panel;
    public GameObject _text;
    private char _char = 'O';
    public GameObject _joystick;

    public void Start()
    {
        _panel.GetComponent<Exp_Controller>().Parse();
    }
    public void OnOff()
    {
        _joystick.SetActive(_panel.activeInHierarchy);
        _panel.SetActive(!_panel.activeInHierarchy);
        if (_char == 'O')            _char = 'X';
        else                         _char = 'O';
        _text.GetComponent<Text>().text = _char.ToString();
    }
}
