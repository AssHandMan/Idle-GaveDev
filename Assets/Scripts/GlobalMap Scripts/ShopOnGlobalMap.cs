using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class ShopOnGlobalMap : MonoBehaviour
{
    public GlobalMapMainController controller;
    public int lvl, perSecReward, MaxStorage, ShopNumber;
    public string ShopName, SceneName;
    public bool isBuy;
    public GameObject NumberShopUI, ClosedObj, hereTxt, CollectBtn;
    public float MoneyBreake;
    public Text MoneyCount;

    //floor, walls, rack, arcade1, arcade2, gamepad
    public MeshRenderer[] ShopMeshes;
    public Material[] ShopMaterials;

    private DateTime serverTime;
    private bool serverTimeReady = false;
    private double cachedTimeAway = 0;


    void Start()
    {
        if (SceneManager.GetActiveScene().name == SceneName)
        {
            hereTxt.SetActive(true);
            ClosedObj.SetActive(false);
        }
        else
        {
            hereTxt.SetActive(false);
            if (isBuy) { StartEarningOfflineMoney(); }
        }
        if (SceneName == "TheFirstShop")
        {
            StartCoroutine(GetServerTime((serverTime) => {
                if (isBuy)
                {
                    double secondsAway = CheckLastLogin(serverTime);
                    cachedTimeAway = secondsAway;
                    if (secondsAway >= 10)
                    {
                        MoneyBreake = (float)(secondsAway / 10.0 * perSecReward);
                        if (MoneyBreake > MaxStorage) { MoneyBreake = MaxStorage; }
                        MoneyCount.text = MoneyBreake.ToString();
                        CollectBtn.SetActive(true);
                    }
                }
            }));
        }

    }

    void Update()
    {
        ClosedObj.transform.LookAt(Camera.main.transform.position);
        hereTxt.transform.LookAt(Camera.main.transform.position);
        CollectBtn.transform.LookAt(Camera.main.transform.position);
    }

    private void OnMouseUp()
    {
        if (controller.playerLvl.lvl >= lvl)
        {
            if (isBuy)
            {
                controller.ShowStatsShop(MaxStorage, perSecReward, SceneName);
            }
            else
            {
                controller.ShowOpenMessege(ShopName, ShopNumber, this);
            }
        }
        else
        {
            controller.ShowCloseMessege(ShopName);
        }
    }

    public void OpenShop()
    {
        isBuy = true;
        for (int i = 0; i < ShopMeshes.Length; i++)
        {
            ShopMeshes[i].material = ShopMaterials[i];
        }
        NumberShopUI.SetActive(true);
        ClosedObj.SetActive(false);
        if (SceneManager.GetActiveScene().name != SceneName)
        {
            StartEarningOfflineMoney();
        }
    }
        
    public void LoadShop()
    {
        OpenShop();
        //Debug.Log(gameObject.name + "!!!!!!!!!!");
        StartCoroutine(GetServerTime((serverTime) => {
            if (isBuy)
            {
                double secondsAway = CheckLastLogin(serverTime);
                cachedTimeAway = secondsAway;
                if (secondsAway >= 10)
                {
                    MoneyBreake = (float)(secondsAway / 10.0 * perSecReward);
                    if (MoneyBreake > MaxStorage) { MoneyBreake = MaxStorage; }
                    MoneyCount.text = MoneyBreake.ToString();
                    CollectBtn.SetActive(true);
                }
            }
        }));
    }

    private double CheckLastLogin(DateTime now)
    {
        if (PlayerPrefs.HasKey("LustLoginKey"))
        {
            string savedTime = PlayerPrefs.GetString("LustLoginKey");
            DateTime lastLogin = DateTime.Parse(savedTime, null, System.Globalization.DateTimeStyles.RoundtripKind);
            TimeSpan timeAway = now - lastLogin;
            Debug.Log(lastLogin);
            return timeAway.TotalSeconds;
        }
        else
        {
            return 0;
        }
    }

    private void SaveLoginTime(DateTime now)
    {
        PlayerPrefs.SetString("LustLoginKey", now.ToString("o"));
        PlayerPrefs.Save();
    }

    public void StartEarningOfflineMoney()
    {
        StartCoroutine(EarningMoney());
    }

    private IEnumerator EarningMoney()
    {
        yield return new WaitForSecondsRealtime(10);
        MoneyBreake += perSecReward;
        if (MoneyBreake > MaxStorage) { MoneyBreake = MaxStorage; }
        MoneyCount.text = MoneyBreake.ToString();
        CollectBtn.SetActive(true);
        StartCoroutine(EarningMoney());
    }

    public void CollectMoney()
    {
        controller.AddMoney(MoneyBreake);
        MoneyBreake = 0;
        CollectBtn.SetActive(false);
    }

    void OnApplicationQuit()
    {
        if (serverTimeReady)
        {
            SaveLoginTime(serverTime);
        }
    }

    private IEnumerator GetServerTime(Action<DateTime> callback)
    {
        using (UnityWebRequest request = UnityWebRequest.Head("https://www.google.com"))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string dateStr = request.GetResponseHeader("Date");
                if (!string.IsNullOrEmpty(dateStr))
                {
                    serverTime = DateTime.Parse(dateStr).ToUniversalTime();
                    serverTimeReady = true;
                    callback?.Invoke(serverTime);
                }
                else
                {
                    Debug.LogError("Дата не получена");
                }
            }
            else
            {
                Debug.LogError("Ошибка запроса времени: " + request.error);
            }
        }
    }
}
