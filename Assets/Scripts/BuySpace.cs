using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuySpace : MonoBehaviour
{
    public GameObject station;

    public Text PriceText;
    public Image ProgressBar;
    public int price;
    public string type;

    private PlayerMovement plr;
    private bool inProgress;
    private float step;
    private float savedElapsedTime = 0f;    
    private PlayerWallet wallet;
    private UpgreadeSceneMeneger meneger;

    void Start()
    {
        PriceText.text = price.ToString();
        step = price;
        wallet = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerWallet>();
        meneger = GameObject.FindGameObjectWithTag("Meneger").GetComponent<UpgreadeSceneMeneger>();
    }

    void Update()
    {
        if (plr != null)
        {

            if (plr.movementDirection != Vector3.zero && inProgress)
            {
                StopAllCoroutines();
                inProgress = false;
            }
        }


    }
    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<PlayerMovement>() != null)
        {
            plr = other.gameObject.GetComponent<PlayerMovement>();
        }
    }
    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerMovement>() != null)
        {
           if(plr.movementDirection == Vector3.zero && !inProgress && wallet.Money > 0)
            {
                StartCoroutine(Paying());
                inProgress = true;
            }
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerMovement>() != null)
        {
            plr = null;
            inProgress = false;
            StopAllCoroutines();
        }
    }

    public void Buy()
    {
        if (station.name == "Door")
        {
            station.SetActive(false);
        }
        else if (station.name == "Office" || station.name == "Toilet")
        {
            station.SetActive(true);
        }
        else
        {

            if (station.GetComponent<SellStation>() != null)
            {
                station.GetComponent<SellStation>().space = this;
                station.GetComponent<SellStation>().BuyUpgrade();
            }

            else if (station.GetComponent<ArcadeMachine>() != null)
            {
                if (!station.GetComponent<ArcadeMachine>().isToilet)
                {
                    if (!station.activeSelf)
                    {
                        station.SetActive(true);
                        station.GetComponent<ArcadeMachine>().lvl = 1;
                        station.GetComponent<ArcadeMachine>().space = this;
                        ChangePrice(station.GetComponent<ArcadeMachine>().PriceList[0]);
                    }
                    else
                    {
                        station.GetComponent<ArcadeMachine>().BuyUpgrade();
                    }
                }
                else
                {
                    station.GetComponent<ArcadeMachine>().Main.SetActive(true);
                }
            }
            else if (station.GetComponent<CreateStation>() != null)
            {
                if (!station.activeSelf)
                {
                    station.SetActive(true);
                    station.GetComponent<CreateStation>().lvl = 1;
                    station.GetComponent<CreateStation>().space = this;
                    ChangePrice(station.GetComponent<CreateStation>().PriceList[0]);
                }
                else
                {
                    station.GetComponent<CreateStation>().BuyUpgrade();
                }
            }
            else if (station.GetComponent<WorkStation>() != null)
            {
                station.GetComponent<WorkStation>().space = this;
                station.GetComponent<WorkStation>().BuyUpgrade();
            }
            else if (station.GetComponent<CarSellStation>() != null)
            {
                station.GetComponent<CarSellStation>().space = this;
                station.GetComponent<CarSellStation>().BuyUpgrade();
            }
        }

        meneger.LevelUp(this);

        gameObject.SetActive(false);
    }

    private IEnumerator Paying()
    {
        float totalTime = 2f;
        float elapsedTime = savedElapsedTime; // Загружаем сохранённое время
        int startPrice = price;

        while (price > 0 && wallet.Money > 0)
        {
            elapsedTime += Time.deltaTime;
            savedElapsedTime = elapsedTime; // Сохраняем прогресс

            float progress = Mathf.Clamp01(elapsedTime / totalTime);

            int amountToDeduct = Mathf.Min(Mathf.CeilToInt(startPrice * progress) - (startPrice - price), wallet.Money, price);

            if (amountToDeduct > 0)
            {
                wallet.ChangeCount(-amountToDeduct, "Money");
                price -= amountToDeduct;
                ProgressBar.fillAmount = 1f - ((float)price / startPrice);
                PriceText.text = price.ToString();
            }

            yield return null;
        }

        if (price <= 0)
        {
            Buy();
            savedElapsedTime = 0f; // Сбрасываем сохранённый прогресс после покупки
        }
    }

    public void ChangePrice(int count)
    {
        ProgressBar.fillAmount = 0;
        price = count;
        PriceText.text = price.ToString();
        step = price;
    }
}