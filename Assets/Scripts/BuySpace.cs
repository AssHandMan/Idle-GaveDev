using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UIElements.VisualElement;

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
    private PlayerWallet wallet;
    private UpgreadeSceneMeneger meneger;

    void Start()
    {
        PriceText.text = price.ToString();
        step = price;
        wallet = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerWallet>();
        meneger = GameObject.FindGameObjectWithTag("Meneger").GetComponent<UpgreadeSceneMeneger>();
    }

    // Update is called once per frame
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

    private IEnumerator Paying()
    {
        if (price > 0)
        {
            if (wallet.Money > 0)
            {
                yield return new WaitForSeconds(0.01f);
                price--;
                ProgressBar.fillAmount += (1 / step);
                PriceText.text = price.ToString();

                wallet.ChangeCount(-1, "Money");
            }
            else
            {
                yield return new WaitForSeconds(0.5f);
                StartCoroutine(Paying());
            }
        }
        else
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
    }
    public void ChangePrice(int count)
    {
        ProgressBar.fillAmount = 0;
        price = count;
        PriceText.text = price.ToString();
        step = price;
    }
}
