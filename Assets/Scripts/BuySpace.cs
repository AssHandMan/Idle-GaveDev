using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuySpace : MonoBehaviour
{
    public GameObject station;

    public Text PriceText;
    public Image ProgressBar;
    public int price, startPrice;
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
        startPrice = price;
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

    public void Buy(bool forAll)
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

        if (!forAll) { meneger.LevelUp(this); }

        gameObject.SetActive(false);
    }

    private IEnumerator Paying()
    {
        // Инициализируем переменные
        float elapsedTime = savedElapsedTime;
        float totalTime = 0f;

        // Устанавливаем время в зависимости от цены плитки
        if (startPrice == 5)
        {
            totalTime = 0.3f; // Заполнение за 2 секунды для цены 5
        }
        else if (startPrice == 25)
        {
            totalTime = 1f; // Заполнение за 3 секунды для цены 25
        }
        else
        {
            totalTime = 2f; // Для других цен оставляем стандартное время
        }

        while (price > 0 && wallet.Money > 0)
        {
            elapsedTime += Time.deltaTime;
            savedElapsedTime = elapsedTime; // Сохраняем прогресс времени

            float progress = Mathf.Clamp01(elapsedTime / totalTime); // Нормализуем прогресс

            int amountToDeduct = Mathf.Min(Mathf.CeilToInt(startPrice * progress) - (startPrice - price), wallet.Money, price);

            if (amountToDeduct > 0)
            {
                wallet.ChangeCount(-amountToDeduct, "Money");
                price -= amountToDeduct;
                ProgressBar.fillAmount = 1f - ((float)price / startPrice); // Обновляем прогресс-бар
                PriceText.text = price.ToString();
            }

            yield return null;
        }

        if (price <= 0)
        {
            Buy(false); // Если цена стала 0, совершаем покупку
            savedElapsedTime = 0f; // Сбрасываем сохранённое время
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