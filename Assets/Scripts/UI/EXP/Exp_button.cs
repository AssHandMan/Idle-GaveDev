using UnityEngine;
using UnityEngine.UI;

public class Exp_button : MonoBehaviour
{
    public GameObject _panel;
    public GameObject _joystick;
    [SerializeField] private GameObject _destination;

    public void Start()
    {
        _panel.GetComponent<Exp_Controller>().Parse();
    }
    public void On()
    {
        _joystick.SetActive(false);
        _panel.SetActive(true);
        this.gameObject.SetActive(false);
        _destination.SetActive(false);
    }
    public void Off()
    {
        _joystick.SetActive(true);
        _panel.SetActive(false);
        this.gameObject.SetActive(true);
        _destination.SetActive(true);

    }
}
