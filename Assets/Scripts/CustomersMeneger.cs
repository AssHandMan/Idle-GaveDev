using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CustomersMeneger : MonoBehaviour
{
    public Canvas canvas;

    public GameObject[] PoorCustomer, RichCustomer, Bloger;
    public GameObject Car, Delivery;
    private string[] CustomerType = { "Poor", "Rich", "Bloger" };
    public int CountCustomer, MaxCustomer, MaxCars, MaxKioskCustomers;
    public float CustomerCreateTime;
    public Transform CustomerPos, DeletePos, CarPlace, DeliveryPlace;

    public int CountOfCars;
    public string[] QueueTypes;

    public SellStation CashMachine1, CashMachine2, kiosk;
    public CarSellStation CarCashMachine;
    public GameObject[] ArcadeMachines;
    public GameObject[] Toilets;
    private GameObject DeliveryNow;

    private int a;

    void Start()
    {
        StartCoroutine(DefinQueue());
        StartCoroutine(DeliverManCreateProcces());
    }

    void Update()
    {

    }

    public void CreateCustomer(SellStation station)
    {
        int Type = Random.Range(0, 3);
        GameObject customer;
        Customer _customer;
        if (Type == 0)
        {
            customer = Instantiate(PoorCustomer[Random.Range(0, PoorCustomer.Length)], CustomerPos.position, CustomerPos.rotation);
            _customer = customer.GetComponent<Customer>();
            _customer.BuyObjCount = Random.Range(1, 4);

        }
        else if (Type == 1)
        {
            customer = Instantiate(RichCustomer[Random.Range(0, RichCustomer.Length)], CustomerPos.position, CustomerPos.rotation);
            _customer = customer.GetComponent<Customer>();
            _customer.BuyObjCount = Random.Range(4, 7);
        }
        else
        {
            customer = Instantiate(Bloger[Random.Range(0, Bloger.Length)], CustomerPos.position, CustomerPos.rotation);
            _customer = customer.GetComponent<Customer>();
            _customer.BuyObjCount = Random.Range(7, 11);
        }
        customer.transform.SetParent(canvas.transform, true);
        _customer.DeletePlace = DeletePos;
        _customer.target = "Buy";
        _customer.st = station;
        _customer.meneger = this;
        station.Queue.Append(customer);
        station.QueueList.Add(customer);
        station.MakeArray();
        if (station == kiosk) { _customer.kiosk = true; }

        if (station == CashMachine1)
        {
            customer.name = a.ToString();
            a++;
        }
    }

    public void CreateCar()
    {
        GameObject car = Instantiate(Car, CarPlace.transform.position, CarPlace.transform.rotation);
        car.transform.SetParent(canvas.transform, true);
        car.GetComponent<CarCustomer>().meneger = this;
        car.GetComponent<CarCustomer>().BuyObjCount = Random.Range(4, 8);
        car.GetComponent<CarCustomer>().station = CarCashMachine;
        CountOfCars++;
    }

    public void CreateDeliverMan()
    {
        DeliveryNow = Instantiate(Delivery, CustomerPos.transform.position, CustomerPos.transform.rotation);
        DeliveryNow.transform.SetParent(canvas.transform, true);
        DeliveryNow.GetComponent<DeliveryMan>().place = DeliveryPlace;
        DeliveryNow.GetComponent<DeliveryMan>().BuyObjCount = Random.Range(3, 10);
        DeliveryNow.GetComponent<DeliveryMan>().delete = DeletePos;

    }

    private IEnumerator DefinQueue()
    {
        if (CashMachine1.CanWork && CashMachine1.Queue.Length < MaxCustomer)
        {
            
            if (CashMachine1.LustCustomer == null)
            {
                yield return new WaitForSeconds(CustomerCreateTime);
                CreateCustomer(CashMachine1);
            }
            else
            {
                yield return new WaitForSeconds(CustomerCreateTime);
            }
        }
        else
        {
            yield return new WaitForSeconds(CustomerCreateTime);
        }

        if (CarCashMachine.CanWork && CountOfCars < MaxCars)
        {
            yield return new WaitForSeconds(CustomerCreateTime);
            CreateCar();
        }
        else
        {
            yield return new WaitForSeconds(CustomerCreateTime);
        }


        if (CashMachine2.CanWork && CashMachine2.Queue.Length < MaxCustomer)
        {
            if (CashMachine2.LustCustomer == null)
            {
                yield return new WaitForSeconds(CustomerCreateTime);
                CreateCustomer(CashMachine2);
            }
            else
            {
                yield return new WaitForSeconds(CustomerCreateTime);
            }
        }
        else
        {
            yield return new WaitForSeconds(CustomerCreateTime);
        }

        if (kiosk.CanWork && kiosk.Queue.Length < MaxKioskCustomers)
        {
            yield return new WaitForSeconds(CustomerCreateTime);
            CreateCustomer(kiosk);
        }
        else
        {
            yield return new WaitForSeconds(CustomerCreateTime);
        }


        if (CashMachine1.Queue.Length == MaxCustomer && CashMachine2.Queue.Length == MaxCustomer && CountOfCars == MaxCars)
        {
            yield return new WaitForSeconds(3);
            StartCoroutine(DefinQueue());
        }
        else
        {
            StartCoroutine(DefinQueue());
        }
    }
    private IEnumerator DeliverManCreateProcces()
    {
        if (DeliveryNow == null) { CreateDeliverMan(); }
        yield return new WaitForSeconds(20);
        StartCoroutine(DeliverManCreateProcces());
    }
}
