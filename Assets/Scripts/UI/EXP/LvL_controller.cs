using UnityEngine;
using UnityEngine.UI;

public class LvL_controller : MonoBehaviour
{
    [SerializeField] private Text _LvlText;
    [SerializeField] private Exp_Controller _Exp_contr;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Check();
    }
    private void Check()
    {
        _LvlText.text = PlayerPrefs.GetInt("PlayerLVL", 1).ToString();
        _Exp_contr.CheckLvL();
    }
}
