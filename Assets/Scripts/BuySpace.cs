using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UIElements.VisualElement;

public class BuySpace : MonoBehaviour
{
    public GameObject[] BuyObject;
    public GameObject NowStation, NextBuy;
    public Transform CreatePlace, UpgrapdeSpacePlace;
    public Text PriceText;
    public Image ProgressBar;
    public int[] priceList;
    public int price;

    private PlayerMovement plr;
    private bool inProgress;
    private float step;
    private int UpgrageStatement;
    private PlayerWallet wallet;

    void Start()
    {
        price = priceList[0];
        PriceText.text = price.ToString();
        step = price;
        wallet = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerWallet>();
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
           if(plr.movementDirection == Vector3.zero && !inProgress)
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
            StartCoroutine(Paying());
        }
        else
        {
            if(NextBuy != null) { NextBuy.SetActive(true); }
            if (NowStation != null) { Destroy(NowStation); }
            NowStation = Instantiate(BuyObject[UpgrageStatement], CreatePlace.position, BuyObject[UpgrageStatement].transform.rotation);
            if (UpgrageStatement < BuyObject.Length-1)
            {
                UpgrageStatement++;
                
                price = priceList[UpgrageStatement];
                step = price;
                PriceText.text = price.ToString();
                ProgressBar.fillAmount = 0;
            }
            else
            {
                Destroy(gameObject);
            }

        }
    }
}
