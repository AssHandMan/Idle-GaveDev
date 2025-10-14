using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomersMeneger : MonoBehaviour
{
    public Canvas canvas;

    public GameObject CustomerPrefab;
    public GameObject Car, Delivery;
    private string[] CustomerType = { "Poor", "Rich", "Bloger" };
    public int CountCustomer, MaxCustomer, MaxCars, MaxKioskCustomers;
    public float CustomerCreateTimeMin, CustomerCreateTimeMax, DeliveryManCreateTime, timeEventIntervalCall, timeEventIntervalSleep;
    public Transform CustomerPos, DeletePos, CarPlace, DeliveryPlace;

    public int CountOfCars;

    public SellStation CashMachine1, CashMachine2, kiosk;
    public CarSellStation CarCashMachine;
    public GameObject[] ArcadeMachines;
    public GameObject[] Toilets;
    private GameObject DeliveryNow;
    private List<string> QueueTypes;
    public List<Customer> ReadyforSleep;

    private int a;

    void Start()
    {
        StartCoroutine(DefinQueue());
        StartCoroutine(MakeCall());
        StartCoroutine(DoSleep());
    }

    void Update()
    {

    }
    public void CreateCustomer(SellStation station)
    {
        int Type = Random.Range(0, 3);
        GameObject customer;
        Customer _customer;
        customer = Instantiate(CustomerPrefab, CustomerPos.position, CustomerPos.rotation);
        _customer = customer.GetComponent<Customer>();
        _customer.ChangeType(Type);

        customer.transform.SetParent(canvas.transform, true);
        _customer.DeletePlace = DeletePos;
        _customer.target = "Buy";
        _customer.st = station;
        _customer.meneger = this;
        station.Queue.Append(customer);
        station.QueueList.Add(customer);
        station.MakeArray();
        if(station == CashMachine1) { _customer.Cash1ProductIcon.SetActive(true); }
        else if (station == CashMachine2) { _customer.Cash2ProductIcon.SetActive(true); }
        else { _customer.KioskProductIcon.SetActive(true); }
        if (station == kiosk) { _customer.kiosk = true; }
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
        if(CashMachine1.CanWork)
        {
            DeliveryNow = Instantiate(Delivery, CustomerPos.transform.position, CustomerPos.transform.rotation);
            DeliveryNow.transform.SetParent(canvas.transform, true);
            DeliveryNow.GetComponent<DeliveryMan>().place = DeliveryPlace;
            DeliveryNow.GetComponent<DeliveryMan>().BuyObjCount = Random.Range(3, 10);
            DeliveryNow.GetComponent<DeliveryMan>().delete = DeletePos;
        }

    }

    public bool CheckLustCustomer(SellStation st)
    {
        int a = 0;

        for (int i = 0; i < st.Queue.Length; i++)
        {
            if (st.Queue[i].GetComponent<Customer>().targetPos == st.CustomerPlace[9])
            {
                a++;
                break;
            }
        }
        if(a>0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private IEnumerator DefinQueue()
    {
        #region old
        /*
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
        }*/
        #endregion

        QueueTypes = new List<string>();

        if (CashMachine1.CanWork && CashMachine1.Queue.Length < MaxCustomer && !CheckLustCustomer(CashMachine1)) { QueueTypes.Add("Cash1"); }
        if (CarCashMachine.CanWork && CountOfCars < MaxCars) { QueueTypes.Add("Car"); }
        if (CashMachine2.CanWork && CashMachine2.Queue.Length < MaxCustomer && !CheckLustCustomer(CashMachine2)) { QueueTypes.Add("Cash2"); }
        if (kiosk.CanWork && kiosk.Queue.Length < MaxKioskCustomers) { QueueTypes.Add("Kiosk"); }

        if (QueueTypes.Count > 0)
        {
            for (int i = QueueTypes.Count; i > 0; i--)
            {
                string type = QueueTypes[Random.Range(0, QueueTypes.Count)];
                if (type == "Cash1") { CreateCustomer(CashMachine1); }
                else if (type == "Cash2") { CreateCustomer(CashMachine2); }
                else if (type == "Kiosk") { CreateCustomer(kiosk); }
                else { CreateCar(); }
                QueueTypes.Remove(type);
                yield return new WaitForSeconds(Random.Range(CustomerCreateTimeMin, CustomerCreateTimeMax));
            }
            StartCoroutine(DefinQueue());
        }
        else
        {
            yield return new WaitForSeconds(Random.Range(CustomerCreateTimeMin, CustomerCreateTimeMax));
            StartCoroutine(DefinQueue());
        }
    }

    public void DeliveryStart()
    {
        StartCoroutine(DeliverManCreateProcces());
    }
    private IEnumerator DeliverManCreateProcces()
    {
        if (DeliveryNow == null) { CreateDeliverMan(); }
        yield return new WaitForSeconds(DeliveryManCreateTime);
        StartCoroutine(DeliverManCreateProcces());
    }
    public int GetPosinQueue(GameObject obj, SellStation station)
    {
        int a = 0;

        for (int i = 0; i < station.Queue.Length; i++)
        {
            if (station.Queue[i] == obj)
            {
                a = i;
                break;
            }
        }
        return a;
    }
    private IEnumerator MakeCall()
    {
        yield return new WaitForSeconds(timeEventIntervalCall);
        if (!CashMachine1.isCalling && !CashMachine2.isCalling)
        {
            List<SellStation> StationsList = new List<SellStation>();
            if (CashMachine1.CanWork && CashMachine1.Queue.Length > 0 && !CashMachine1.isCalling) { StationsList.Add(CashMachine1); }
            if (CashMachine2.CanWork && CashMachine2.Queue.Length > 0 && !CashMachine2.isCalling) { StationsList.Add(CashMachine2); }
            SellStation station = StationsList[Random.Range(0, StationsList.Count)];
            if (station.Queue.Length > 0)
            {

                if (!station.isCalling)
                {
                    GameObject customer = station.Queue[Random.Range(0, station.Queue.Length)];
                    Customer _customer = customer.GetComponent<Customer>();
                    if (_customer.target != null)
                    {
                        if (_customer.transform.position.x == _customer.targetPos.position.x && _customer.transform.position.z == _customer.targetPos.position.z)
                        {
                            if (!_customer.isCalling)
                            {
                                _customer.MakeCall();
                                station.isCalling = true;
                                station.CallingCustomer = _customer;
                                station.LustCustomerPosinQueue = GetPosinQueue(customer, station);
                            }
                        }
                    }
                }
            }
        }
        StartCoroutine(MakeCall());
    }

    private IEnumerator DoSleep()
    {
        yield return new WaitForSeconds(timeEventIntervalSleep);
        if(ReadyforSleep.Count > 0)
        {
            int count = 0;
            for(int i = 0; i < ReadyforSleep.Count; i++)
            {
                if (ReadyforSleep[i].isSleeping == true)
                {
                    count++;
                    break;
                }
            }
            if (count == 0)
            {
                int n = Random.Range(0, ReadyforSleep.Count);
                ReadyforSleep[n].Sleep();
                ReadyforSleep[n]._animator.SetBool("IsPlay", false);
                ReadyforSleep[n]._animator.SetBool("IsDance", false);
                ReadyforSleep[n]._animator.SetBool("IsMoving", false);


            }
        }
        StartCoroutine(DoSleep());
    }
}
