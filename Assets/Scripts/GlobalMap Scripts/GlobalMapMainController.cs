using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GlobalMapMainController : MonoBehaviour
{
    public ExpLvlController playerLvl;

    public GameObject MessegeClose;
    public Text MessegeCloseShopName;

    public GameObject MessegeOpen;
    public Text MessegeOpeShopName, countShopsTxt;
    private int ShopToOpen, CountShops = 1;
    public bool[] OpenShops;
    private ShopOnGlobalMap shopNow;

    public GameObject StatsShopMenu, StorageEng, StorageRus;
    public Text MaxStorage, PerSec;
    public string sceneToGo;

    public PlayerWallet wallet;

    [SerializeField] private GameObject UiShowHide, Money, Diamonds, Skins, UiGlobalMap;
    [Header("Audio")]
    public AudioSource _audio;
    public AudioSource _audio2;
    public AudioClip _clipBtn;
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    public void CloseMessege()
    {
        MessegeClose.SetActive(false);
        MessegeOpen.SetActive(false);
    }

    public void ShowCloseMessege(string shopName)
    {
        MessegeCloseShopName.text = shopName;
        MessegeClose.SetActive(true);
    }

    public void ShowOpenMessege(string shopName, int ShopNumber, ShopOnGlobalMap shop)
    {
        MessegeOpeShopName.text = shopName;
        MessegeOpen.SetActive(true);
        ShopToOpen = ShopNumber;
        shopNow = shop;
    }

    public void OpenNewShop()
    {
        shopNow.OpenShop();
        OpenShops[ShopToOpen] = true;
        shopNow = null;
        ShopToOpen = -1;
        CountShops++;
        countShopsTxt.text = CountShops.ToString();
        CloseMessege();
    }

    public void ShowStatsShop(int max, int per, string sceneName)
    {
        MaxStorage.text = Convert.ToString(max);
        PerSec.text = Convert.ToString(per);
        StatsShopMenu.SetActive(true);
        sceneToGo = sceneName;
    }
    public void CloseShopStats()
    {
        StatsShopMenu.SetActive(false);
    }

    public void EnterShop()
    {
        if(SceneManager.GetActiveScene().name != sceneToGo)
        {
            SceneManager.LoadScene(sceneToGo);
        }
        else
        {
            CloseGlobalMapMenu();
        }
    }

    public void AddMoney(float count)
    {
        wallet.ChangeCount(count, "Money");
    }
    public void CloseGlobalMapMenu()
    {
        _audio.Stop();
        _audio2.PlayOneShot(_clipBtn,0.4f);
        _audio2.PlayDelayed(0.4f);
        CloseShopStats();
        CloseMessege();
        UiGlobalMap.SetActive(false);
        UiShowHide.SetActive(true);
        Money.SetActive(true);
        Diamonds.SetActive(true);
        Skins.SetActive(true);
        Camera.main.GetComponent<CameraController>().SetPlayMood();
    }
}
