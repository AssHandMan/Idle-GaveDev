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
    public ShopOnGlobalMap[] shops;
    private ShopOnGlobalMap shopNow;

    public GameObject StatsShopMenu, StorageEng, StorageRus;
    public Text MaxStorage, PerSec;
    public string sceneToGo;

    public PlayerWallet wallet;

    [SerializeField] private GameObject UiShowHide, Money, Diamonds, Skins, UiGlobalMap;
    void Start()
    {
        Load();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.U))
        {
            Save();
        }
    }

    public void Load()
    {
        GlobalMapData data = SaveMeneger.LoadGlobalMap();
        if (data != null)
        {
            Debug.Log(data.OpenShops[0].ToString() + data.OpenShops[1].ToString() + data.OpenShops[2].ToString());
            for (int i = 1; i < data.OpenShops.Length; i++)
            {
                if (data.OpenShops[i]) 
                {
                    OpenShops[i] = true;
                    CountShops++;
                    countShopsTxt.text = CountShops.ToString();
                    shops[i].LoadShop();
                }
            }
        }
    }
    public void Save()
    {
        SaveMeneger.SaveGlobalMap(this);
        Debug.Log(OpenShops[0].ToString() + OpenShops[1].ToString() + OpenShops[2].ToString());
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
        Save();
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
