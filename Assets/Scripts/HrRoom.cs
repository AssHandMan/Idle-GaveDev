
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HrRoom : MonoBehaviour
{
    public GameObject menu, buttonsHide, AddBaner;
    public GameObject[] LockCover;
    public GameObject[] Buttons;
    public GameObject[] Menegers;
    public Image[] Jack, Andy, Mag, Bob, Tom, Sam, Dan;
    public Text[] PriceButton, PriceDiamondsButton;
    public TextMeshProUGUI[] LvlTxt;
    public bool[] BoughtMenegres;
    public int[] lvl;
    private float[,] price = { { 100, 200, 300, 400, 500, 600, 700 }, { 100, 200, 300, 400, 500, 600, 700 }, { 100, 200, 300, 400, 500, 600, 700 }, { 100, 200, 300, 400, 500, 600, 700 }, { 100, 200, 300, 400, 500, 600, 700 }, { 100, 200, 300, 400, 500, 600, 700 }, { 100, 200, 300, 400, 500, 600, 700 } };
    private float[,] priceDiamonds = { { 1, 2, 3, 4, 5, 6, 7 }, { 1, 2, 3, 4, 5, 6, 7 }, { 1, 2, 3, 4, 5, 6, 7 }, { 1, 2, 3, 4, 5, 6, 7 }, { 1, 2, 3, 4, 5, 6, 7 }, { 1, 2, 3, 4, 5, 6, 7 }, { 1, 2, 3, 4, 5, 6, 7 } };
    private float[] UpgreadStats = { 8, 4, 8, 11, 7, 15, 14, 10, 22, 18, 13, 30 };
    private PlayerMovement plr;
    private PlayerWallet wallet;

    public Meneger[] menegersScript;


    public List<string> TargetTypes = new List<string>();

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        plr = player.GetComponent<PlayerMovement>();
        wallet = player.GetComponent<PlayerWallet>();

        for(int i = 0; i < PriceDiamondsButton.Length; i++)
        {
            PriceDiamondsButton[i].text = "1";
        }
    }


    void Update()
    {

    }

    public bool CheckWork(string type)
    {


        int count = 0;

        for(int i = 0; i < TargetTypes.Count; i++)
        {
            if(type == TargetTypes[i])
            {
                count++;
                break;
            }
        }
        if(count==0)
        {
            return false;
        }
        else
        {
            return true;
        }

    }

    public void AddTarget(string type)
    {
        TargetTypes.Add(type);
    }

    public void RemoveTarget(string type)
    {
        TargetTypes.Remove(type);
    }

    public void BuyButton(int name)
    {
        if (!BoughtMenegres[name] && wallet.Money >= price[name, lvl[name]])
        {
            if (name == 0 || (name > 0 & BoughtMenegres[name - 1] == true))
            {
                wallet.ChangeCount(-price[name, lvl[name]], "Money");
                BoughtMenegres[name] = true;
                LockCover[name].SetActive(false);
                Menegers[name].SetActive(true);
                PriceButton[name].text = Convert.ToString(price[name, lvl[name] + 1]);
                PriceDiamondsButton[name].text = Convert.ToString(priceDiamonds[name, lvl[name] + 1]);
            }
        }
        else
        {
            if (lvl[name] < 6)
            {
                if (wallet.Money >= price[name, lvl[name] + 1])
                {
                    wallet.ChangeCount(-price[name, lvl[name]+1], "Money");
                    lvl[name]++;
                    if (lvl[name] + 1 <= 6) 
                    { 
                        PriceButton[name].text = Convert.ToString(price[name, lvl[name] + 1]);
                        PriceDiamondsButton[name].text = Convert.ToString(priceDiamonds[name, lvl[name] + 1]);
                    }
                    else
                    {
                        Buttons[name].SetActive(false);
                    }
                    Meneger mg = Menegers[name].GetComponent<Meneger>();

                    switch (name)
                    {
                        case 0:
                            if (lvl[name] == 1 || lvl[name] == 4)
                            {
                                Jack[0].fillAmount += 0.5f;
                                mg.speed = UpgreadStats[lvl[name]];
                            }
                            else if (lvl[name] == 2 || lvl[name] == 5)
                            {
                                Jack[1].fillAmount += 0.5f;
                                mg.capacityUpgreadeBufer = UpgreadStats[lvl[name]];
                            }
                            else
                            {
                                Jack[2].fillAmount += 0.5f;
                                mg.profit = UpgreadStats[lvl[name]];
                            }
                            break;

                        case 1:
                            if (lvl[name] == 1 || lvl[name] == 4)
                            {
                                Andy[0].fillAmount += 0.5f;
                                mg.speed = UpgreadStats[lvl[name]];
                            }
                            else if (lvl[name] == 2 || lvl[name] == 5)
                            {
                                Andy[1].fillAmount += 0.5f;
                                mg.capacityUpgreadeBufer = UpgreadStats[lvl[name]];
                            }
                            else
                            {
                                Andy[2].fillAmount += 0.5f;
                                mg.profit = UpgreadStats[lvl[name]];
                            }
                            break;

                        case 2:
                            if (lvl[name] == 1 || lvl[name] == 4)
                            {
                                Mag[0].fillAmount += 0.5f;
                                mg.speed = UpgreadStats[lvl[name]];
                            }
                            else if (lvl[name] == 2 || lvl[name] == 5)
                            {
                                Mag[1].fillAmount += 0.5f;
                                mg.capacityUpgreadeBufer = UpgreadStats[lvl[name]];
                            }
                            else
                            {
                                Mag[2].fillAmount += 0.5f;
                                mg.profit = UpgreadStats[lvl[name]];
                            }
                            break;

                        case 3:
                            if (lvl[name] == 1 || lvl[name] == 4)
                            {
                                Bob[0].fillAmount += 0.5f;
                                mg.speed = UpgreadStats[lvl[name]];
                            }
                            else if (lvl[name] == 2 || lvl[name] == 5)
                            {
                                Bob[1].fillAmount += 0.5f;
                                mg.capacityUpgreadeBufer = UpgreadStats[lvl[name]];
                            }
                            else
                            {
                                Bob[2].fillAmount += 0.5f;
                                mg.profit = UpgreadStats[lvl[name]];
                            }
                            break;

                        case 4:
                            if (lvl[name] == 1 || lvl[name] == 4)
                            {
                                Tom[0].fillAmount += 0.5f;
                                mg.speed = UpgreadStats[lvl[name]];
                            }
                            else if (lvl[name] == 2 || lvl[name] == 5)
                            {
                                Tom[1].fillAmount += 0.5f;
                                mg.capacityUpgreadeBufer = UpgreadStats[lvl[name]];
                            }
                            else
                            {
                                Tom[2].fillAmount += 0.5f;
                                mg.profit = UpgreadStats[lvl[name]];
                            }
                            break;

                        case 5:
                            if (lvl[name] == 1 || lvl[name] == 4)
                            {
                                Sam[0].fillAmount += 0.5f;
                                mg.speed = UpgreadStats[lvl[name]];
                            }
                            else if (lvl[name] == 2 || lvl[name] == 5)
                            {
                                Sam[1].fillAmount += 0.5f;
                                mg.capacityUpgreadeBufer = UpgreadStats[lvl[name]];
                            }
                            else
                            {
                                Sam[2].fillAmount += 0.5f;
                                mg.profit = UpgreadStats[lvl[name]];
                            }
                            break;

                        case 6:
                            if (lvl[name] == 1 || lvl[name] == 4)
                            {
                                Dan[0].fillAmount += 0.5f;
                                mg.speed = UpgreadStats[lvl[name]];
                            }
                            else if (lvl[name] == 2 || lvl[name] == 5)
                            {
                                Dan[1].fillAmount += 0.5f;
                                mg.capacityUpgreadeBufer = UpgreadStats[lvl[name]];
                            }
                            else
                            {
                                Dan[2].fillAmount += 0.5f;
                                mg.profit = UpgreadStats[lvl[name]];
                            }
                            break;
                    }
                }
            }
        }

        LvlTxt[name].text = "LV." + Convert.ToString(lvl[name]);
    }

    public void BuyDiamondsButton(int name)
    {
        if (!BoughtMenegres[name] && wallet.Diamonds >= priceDiamonds[name, lvl[name]])
        {
            if (name == 0 || (name > 0 & BoughtMenegres[name - 1] == true))
            {
                wallet.ChangeCount(-priceDiamonds[name, lvl[name]], "Diamonds");
                BoughtMenegres[name] = true;
                LockCover[name].SetActive(false);
                Menegers[name].SetActive(true);
                PriceButton[name].text = Convert.ToString(price[name, lvl[name] + 1]);
                PriceDiamondsButton[name].text = Convert.ToString(priceDiamonds[name, lvl[name] + 1]);
            }
        }
        else
        {
            if (lvl[name] < 6)
            {
                if (wallet.Diamonds >= priceDiamonds[name, lvl[name] + 1])
                {
                    wallet.ChangeCount(-priceDiamonds[name, lvl[name]], "Diamonds");
                    lvl[name]++;
                    if (lvl[name] + 1 <= 6)
                    {
                        PriceButton[name].text = Convert.ToString(price[name, lvl[name] + 1]);
                        PriceDiamondsButton[name].text = Convert.ToString(priceDiamonds[name, lvl[name] + 1]);
                    }
                    else
                    {
                        Buttons[name].SetActive(false);
                    }
                    Meneger mg = Menegers[name].GetComponent<Meneger>();

                    switch (name)
                    {
                        case 0:
                            if (lvl[name] == 1 || lvl[name] == 4)
                            {
                                Jack[0].fillAmount += 0.5f;
                                mg.speed = UpgreadStats[lvl[name]];
                            }
                            else if (lvl[name] == 2 || lvl[name] == 5)
                            {
                                Jack[1].fillAmount += 0.5f;
                                mg.capacityUpgreadeBufer = UpgreadStats[lvl[name]];
                            }
                            else
                            {
                                Jack[2].fillAmount += 0.5f;
                                mg.profit = UpgreadStats[lvl[name]];
                            }
                            break;

                        case 1:
                            if (lvl[name] == 1 || lvl[name] == 4)
                            {
                                Andy[0].fillAmount += 0.5f;
                                mg.speed = UpgreadStats[lvl[name]];
                            }
                            else if (lvl[name] == 2 || lvl[name] == 5)
                            {
                                Andy[1].fillAmount += 0.5f;
                                mg.capacityUpgreadeBufer = UpgreadStats[lvl[name]];
                            }
                            else
                            {
                                Andy[2].fillAmount += 0.5f;
                                mg.profit = UpgreadStats[lvl[name]];
                            }
                            break;

                        case 2:
                            if (lvl[name] == 1 || lvl[name] == 4)
                            {
                                Mag[0].fillAmount += 0.5f;
                                mg.speed = UpgreadStats[lvl[name]];
                            }
                            else if (lvl[name] == 2 || lvl[name] == 5)
                            {
                                Mag[1].fillAmount += 0.5f;
                                mg.capacityUpgreadeBufer = UpgreadStats[lvl[name]];
                            }
                            else
                            {
                                Mag[2].fillAmount += 0.5f;
                                mg.profit = UpgreadStats[lvl[name]];
                            }
                            break;

                        case 3:
                            if (lvl[name] == 1 || lvl[name] == 4)
                            {
                                Bob[0].fillAmount += 0.5f;
                                mg.speed = UpgreadStats[lvl[name]];
                            }
                            else if (lvl[name] == 2 || lvl[name] == 5)
                            {
                                Bob[1].fillAmount += 0.5f;
                                mg.capacityUpgreadeBufer = UpgreadStats[lvl[name]];
                            }
                            else
                            {
                                Bob[2].fillAmount += 0.5f;
                                mg.profit = UpgreadStats[lvl[name]];
                            }
                            break;

                        case 4:
                            if (lvl[name] == 1 || lvl[name] == 4)
                            {
                                Tom[0].fillAmount += 0.5f;
                                mg.speed = UpgreadStats[lvl[name]];
                            }
                            else if (lvl[name] == 2 || lvl[name] == 5)
                            {
                                Tom[1].fillAmount += 0.5f;
                                mg.capacityUpgreadeBufer = UpgreadStats[lvl[name]];
                            }
                            else
                            {
                                Tom[2].fillAmount += 0.5f;
                                mg.profit = UpgreadStats[lvl[name]];
                            }
                            break;

                        case 5:
                            if (lvl[name] == 1 || lvl[name] == 4)
                            {
                                Sam[0].fillAmount += 0.5f;
                                mg.speed = UpgreadStats[lvl[name]];
                            }
                            else if (lvl[name] == 2 || lvl[name] == 5)
                            {
                                Sam[1].fillAmount += 0.5f;
                                mg.capacityUpgreadeBufer = UpgreadStats[lvl[name]];
                            }
                            else
                            {
                                Sam[2].fillAmount += 0.5f;
                                mg.profit = UpgreadStats[lvl[name]];
                            }
                            break;

                        case 6:
                            if (lvl[name] == 1 || lvl[name] == 4)
                            {
                                Dan[0].fillAmount += 0.5f;
                                mg.speed = UpgreadStats[lvl[name]];
                            }
                            else if (lvl[name] == 2 || lvl[name] == 5)
                            {
                                Dan[1].fillAmount += 0.5f;
                                mg.capacityUpgreadeBufer = UpgreadStats[lvl[name]];
                            }
                            else
                            {
                                Dan[2].fillAmount += 0.5f;
                                mg.profit = UpgreadStats[lvl[name]];
                            }
                            break;
                    }
                }
            }
        }
        LvlTxt[name].text = "LV." + Convert.ToString(lvl[name]);
    }

    public void OpenClose()
    {
        menu.SetActive(false);
        buttonsHide.SetActive(true);
        AddBaner.SetActive(true);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerMovement>() != null)
        {
            menu.SetActive(true);
            buttonsHide.SetActive(false);
            AddBaner.SetActive(false);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerMovement>() != null)
        {
            menu.SetActive(false);
            buttonsHide.SetActive(true);
            AddBaner.SetActive(true);
        }
    }
}
