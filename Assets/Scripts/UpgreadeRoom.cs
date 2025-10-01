using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class UpgreadeRoom : MonoBehaviour
{
    public GameObject menu;
    public PlayerMovement plr;
    public PlayerWallet wallet;
    public PlayerTakeDropSystem hands;

    public Image SpeedBar;
    public int[] SpeedPrice = { 100, 250, 400, 500, 1000 };
    public int[] SpeedValue = { 7, 9, 11, 13, 15 };

    public Image CapacityBar;
    public int[] CapacityPrice = { 100, 250, 400, 500, 1000 };
    public int[] CapacityValue = { 7, 9, 11, 13, 15 };

    public Image ProfitBar;
    public int[] ProfitPrice = { 100, 250, 400, 500, 1000 };
    public float[] ProfitValue = { 1.2f, 1.6f, 2f, 2.4f, 3f };

    public Text SpeedCount, CapacityCount, ProfitCount;
    private int speed, capacity, profit;
    public int CountAds;
    public GameObject AdsScreen, ButtonsBuySpeed, ButtonsBuyCapacity, ButtonsBuyProfit;
    public GameSettingsController settings;
    private bool canSave;
    public Text AddTime;

    [Header("Audio")]
    public AudioClip _clipBtn;
    public AudioSource _audio;
    
    void Start()
    {
        SpeedCount.text = SpeedPrice[speed].ToString();
        CapacityCount.text = CapacityPrice[capacity].ToString();
        ProfitCount.text = ProfitPrice[profit].ToString();
        canSave = true; 
        Load();
    }

    void Update()
    {

    }

    public void Load()
    {
        PlayerData data = SaveMeneger.LoadPlayer();
        if (data != null)
        {
            
            canSave = false;

           for(int i = 0; i < data.speed; i++)
           {
                wallet.GetMoney(SpeedPrice[speed]);
                BuySpeed(false);
           }
           for(int i = 0; i < data.capacity; i++)
           {
                wallet.GetMoney(CapacityPrice[capacity]);
                BuyCapacity(false);
           }
           for(int i = 0; i < data.profit; i++)
           {
                wallet.GetMoney(ProfitPrice[profit]);
                BuyProfit(false);
           }
            canSave = true;
        }
    }

    public void Save() 
    { 
        SaveMeneger.SavePlayer(this);
    }

    private void BuyAdvertisement()
    {
        if(CountAds > 0)
        {
            CountAds--;
            StartCoroutine(TimeToEndAds());
        }
    }

    private IEnumerator TimeToEndAds()
    {
        int t = 5;
        AddTime.text = t.ToString();
        AdsScreen.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        for(int i = 0; i < 5; i++)
        {
            yield return new WaitForSeconds(1);
            t--;
            AddTime.text = t.ToString();
        }
        AdsScreen.SetActive(false);
    }

    public void BuySpeed(bool isAdButton)
    {

        if(speed < 5)
        {
            if(isAdButton)
            {
                if(CountAds > 0)
                {
                    plr.speed = SpeedValue[speed];
                    SpeedBar.fillAmount += 0.2f;
                    speed++;
                    if (speed == 5) { ButtonsBuySpeed.SetActive(false); }
                    else { SpeedCount.text = SpeedPrice[speed].ToString(); }
                    BuyAdvertisement();
                    if (canSave) { Save(); }
                }
            }
            else
            {
                if (wallet.Money >= SpeedPrice[speed])
                {
                    wallet.ChangeCount(-SpeedPrice[speed], "Money");
                    plr.speed = SpeedValue[speed];
                    SpeedBar.fillAmount += 0.2f;
                    speed++;
                    if (speed == 5) { ButtonsBuySpeed.SetActive(false); }
                    else { SpeedCount.text = SpeedPrice[speed].ToString(); }
                    if (canSave) { Save(); }
                }
            }
            _audio.PlayOneShot(_clipBtn, 0.4f);
        }
    }

    public void BuyCapacity(bool isAdButton)
    {
        if (capacity < 5)
        {
            if (isAdButton)
            {
                if (CountAds > 0)
                {
                    hands.MaxCount = CapacityValue[capacity];
                    CapacityBar.fillAmount += 0.2f;
                    capacity++;
                    if (capacity == 5) { ButtonsBuyCapacity.SetActive(false); }
                    else { CapacityCount.text = CapacityPrice[capacity].ToString(); }
                    BuyAdvertisement();
                    if (canSave) { Save(); }
                }
            }
            else
            {
                if (wallet.Money >= CapacityPrice[capacity])
                {
                    wallet.ChangeCount(-CapacityPrice[capacity], "Money");
                    hands.MaxCount = CapacityValue[capacity];
                    CapacityBar.fillAmount += 0.2f;
                    capacity++;
                    if (capacity == 5) { ButtonsBuyCapacity.SetActive(false); }
                    else { CapacityCount.text = CapacityPrice[capacity].ToString(); }
                    if (canSave) { Save(); }
                }
            }
            _audio.PlayOneShot(_clipBtn, 0.4f);
        }
    }

    public void BuyProfit(bool isAdButton)
    {
        if (profit < 5)
        {
            if (isAdButton)
            {
                if (CountAds > 0)
                {
                    wallet.MoneyMultiplier = ProfitValue[profit];
                    ProfitBar.fillAmount += 0.2f;
                    profit++;
                    if (profit == 5) { ButtonsBuyProfit.SetActive(false); }
                    else { ProfitCount.text = ProfitPrice[profit].ToString(); }
                    BuyAdvertisement();
                    if (canSave) { Save(); }
                }
            }
            else
            {
                if (wallet.Money >= ProfitPrice[profit])
                {
                    wallet.ChangeCount(-ProfitPrice[profit], "Money");
                    wallet.MoneyMultiplier = ProfitValue[profit];
                    ProfitBar.fillAmount += 0.2f;
                    profit++;
                    if (profit == 5) { ButtonsBuyProfit.SetActive(false); }
                    else { ProfitCount.text = ProfitPrice[profit].ToString(); }
                    if (canSave) { Save(); }
                }
            }
            _audio.PlayOneShot(_clipBtn, 0.4f);
        }
    }

    public void OpenClose()
    {
        menu.SetActive(false);
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<PlayerMovement>() != null)
        {
            menu.SetActive(true);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerMovement>() != null)
        {
            menu.SetActive(false);
        }
    }

    public int Get(string s)
    {
        if(s == "s") { return speed; }
        else if(s == "p") { return profit; }
        else { return capacity; }
    }
}
