using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuySpace : MonoBehaviour
{
    public GameObject station, money;
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
    private Animator anim;
    private Transform plrPos;

    void Start()
    {
        PriceText.text = price.ToString();
        step = price;
        startPrice = price;
        wallet = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerWallet>();
        meneger = GameObject.FindGameObjectWithTag("Meneger").GetComponent<UpgreadeSceneMeneger>();
        anim = GetComponent<Animator>();
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
            anim.SetBool("isBigger", true);
            plr = other.gameObject.GetComponent<PlayerMovement>();
            plrPos = other.gameObject.transform;
        }
    }
    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerMovement>() != null)
        {
           if(plr.movementDirection == Vector3.zero && !inProgress && wallet.Money > 0)
            {
                StartCoroutine(Paying());
                StartCoroutine(MoneyEffectGenerator());
                inProgress = true;
            }
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerMovement>() != null)
        {
            anim.SetBool("isBigger", false);
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
                        station.GetComponent<Animator>().SetTrigger("Buy");
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
        float elapsedTime = savedElapsedTime;
        float totalTime = 0f;

        if (startPrice == 5)
        {
            totalTime = 0.3f;
        }
        else if (startPrice == 25)
        {
            totalTime = 1f;
        }
        else
        {
            totalTime = 2f;
        }

        while (price > 0 && wallet.Money > 0)
        {
            elapsedTime += Time.deltaTime;
            savedElapsedTime = elapsedTime;

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
            Buy(false);
            savedElapsedTime = 0f;
        }
    }
    private IEnumerator MoneyEffectGenerator()
    {
        Vector3 pos = new Vector3(plrPos.position.x, plrPos.position.y + 2f, plrPos.position.z);
        GameObject m = Instantiate(money, pos, money.transform.rotation);
        m.GetComponent<MoneyBuySpace>().GiveMoney(transform.position);
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(MoneyEffectGenerator());
    }


    public void ChangePrice(int count)
    {
        ProgressBar.fillAmount = 0;
        price = count;
        PriceText.text = price.ToString();
        step = price;
    }
}