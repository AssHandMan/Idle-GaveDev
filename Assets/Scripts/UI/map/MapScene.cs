using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private string _gameSceneName = "TheFirstShop";
    [SerializeField] private string _mapSceneName = "MapScene";
    [SerializeField] private GameObject _mainCamera;
    [SerializeField] private GameObject _mainUI;
    [SerializeField] private Scene _scene;
    [SerializeField] private PlayerWallet _PlayerWallet;
    [SerializeField] private NetworkTimeChecker _timeChecker;

    [SerializeField] private GameObject _diamonds;
    [SerializeField] private GameObject _diamonds_pos;
    [SerializeField] private GameObject _diamonds_pos2;

    [SerializeField] private GameObject _mapUI;
    [SerializeField] private GameObject _coins;
    [SerializeField] private GameObject _joystick;

    [SerializeField] private Sprite _map;
    [SerializeField] private Sprite _Exite;
    [SerializeField] private GameObject _text;
    [SerializeField] private Transform _pos2;
    private Transform _pos1;
    private bool _isMapLoaded = false;
    private void Start()
    {
        _pos1 = this.transform;
    }
    public void ToggleMap()
    {
        if (_isMapLoaded)
        {
            UnloadMap();
            UpdateWallet();

        }
        else
        {
            LoadMap();
        }
    }
    private void UpdateWallet()
    {
        for (int i = 0; i < 4; i++)
        {
            if (PlayerPrefs.GetInt("Wallet" + i.ToString(), 0) > 0 && PlayerPrefs.GetInt("Agree" + i.ToString(), 0) == 1)
            {
                _PlayerWallet.ChangeCount(PlayerPrefs.GetInt("Wallet" + i.ToString(), 0), "Money");
                Debug.Log("wallat" + i.ToString() + ": " + PlayerPrefs.GetInt("Wallet" + i.ToString(), 0).ToString());
                PlayerPrefs.SetInt("Wallet" + i.ToString(), 0);
                PlayerPrefs.SetInt("Agree" + i.ToString(), 0);
                PlayerPrefs.Save();
            }
        }
    }
    private void LoadMap()
    {
        SceneManager.LoadSceneAsync(_mapSceneName, LoadSceneMode.Additive);
        _isMapLoaded = true;

        // Можно приостановить игровые процессы, если нужно
        Time.timeScale = 0f;
        _mainCamera.SetActive(false);
        _mainUI.SetActive(false);
        _mapUI.SetActive(true);
        _coins.SetActive(false);
        _joystick.SetActive(false);
        _diamonds.transform.position = _diamonds_pos.transform.position;
        this.GetComponent<Image>().sprite = _Exite;
        _text.SetActive(false);
        this.transform.position=_pos2.position;
    }
    private void UnloadMap()
    {
        SceneManager.UnloadSceneAsync(_mapSceneName);
        _isMapLoaded = false;

        // Возобновляем игру
        Time.timeScale = 1f;
        _mainCamera.SetActive(true);
        _mainUI.SetActive(true);
        _mapUI.SetActive(false);
        _coins.SetActive(true);
        _joystick.SetActive(true);
        _diamonds.transform.position = _diamonds_pos2.transform.position;

        this.GetComponent<Image>().sprite = _map;
        _text.SetActive(true);
        this.transform.position = _pos1.position;
    }
    public void ResetAllPrefs()
    {
        PlayerPrefs.DeleteAll();
        _timeChecker.WriteTimeLastGiveTime("0");
        _timeChecker.WriteTimeLastGiveTime("1");
        _timeChecker.WriteTimeLastGiveTime("2");

    }
}