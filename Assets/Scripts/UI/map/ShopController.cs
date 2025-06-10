using UnityEngine;

public class ShopController : MonoBehaviour
{
    private int _playerLVL;
    [SerializeField] private Shop _shop1;
    [SerializeField] private Shop _shop2;
    [SerializeField] private NetworkTimeChecker _timeChecker;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _playerLVL = PlayerPrefs.GetInt("PlayerLVL",0);
        Debug.Log("PlayerLvl "+ _playerLVL.ToString());
        if (_playerLVL >= 10)
        {
            _shop1.Activated();
            _timeChecker.WriteTimeLastGiveTime("1");
        }
        if (_playerLVL >= 20)
        {
            _shop2.Activated();
            _timeChecker.WriteTimeLastGiveTime("2");
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
