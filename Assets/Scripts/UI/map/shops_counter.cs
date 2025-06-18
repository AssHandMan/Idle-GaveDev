using UnityEngine;
using UnityEngine.UI;

public class shops_counter : MonoBehaviour
{
    [SerializeField] private Text _text;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (PlayerPrefs.GetInt("PlayerLVL") >= 20)
            _text.text = "3";
        else if(PlayerPrefs.GetInt("PlayerLVL") >= 10)
            _text.text = "2";
        else
            _text.text = "1";
    }
}
