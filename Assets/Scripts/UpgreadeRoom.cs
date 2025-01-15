using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    void Start()
    {
        SpeedCount.text = SpeedPrice[speed].ToString();
        CapacityCount.text = CapacityPrice[capacity].ToString();
        ProfitCount.text = ProfitPrice[profit].ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BuySpeed()
    {
        if (speed < 5 && wallet.Money >= SpeedPrice[speed])
        {
            wallet.ChangeCount(-SpeedPrice[speed], "Money");
            plr.speed = SpeedValue[speed];
            SpeedBar.fillAmount += 0.2f;
            speed++;
            if(speed == 5) { SpeedCount.text = "Complete"; }
            else { SpeedCount.text = SpeedPrice[speed].ToString(); }
        }
    }

    public void BuyCapacity()
    {
        if (capacity < 5 && wallet.Money >= CapacityPrice[capacity])
        {
            wallet.ChangeCount(-CapacityPrice[capacity], "Money");
            hands.MaxCount = CapacityValue[capacity];
            CapacityBar.fillAmount += 0.2f;
            capacity++;
            if (capacity == 5) { CapacityCount.text = "Complete"; }
            else { CapacityCount.text = CapacityPrice[capacity].ToString(); }
        }
    }

    public void BuyProfit()
    {
        if (profit < 5 && wallet.Money >= ProfitPrice[profit])
        {
            wallet.ChangeCount(-ProfitPrice[profit], "Money");
            wallet.MoneyMultiplier = ProfitValue[profit];
            ProfitBar.fillAmount += 0.2f;
            profit++;

            if (profit == 5) { ProfitCount.text = "Complete"; }
            else { ProfitCount.text = ProfitPrice[profit].ToString(); }
        }
    }

    public void OpenClose()
    {
        if(menu.activeSelf) { menu.SetActive(false); }
        else { menu.SetActive(true); }
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<PlayerMovement>() != null)
        {
            OpenClose();
        }
    }


}
