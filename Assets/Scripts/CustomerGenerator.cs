using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class CustomerGenerator : MonoBehaviour
{
    public Canvas canvas;
    public GameObject[] Customers;
    
    public GameObject Car, DeliveryMan;
    public int CountOfCars;
    public CarSellStation CarSellStation;
    public Transform CarPlace, DeliveryPlace, DeletePlace;

    private GameObject CustomerNow, DeliveryManNow;

    void Start()
    {
        StartCoroutine(CreateCar());
        //StartCoroutine(CreateCustomer());
    }

    
    void Update()
    {
        if (CustomerNow == null)
        {
            GameObject client = Instantiate(Customers[Random.Range(0, Customers.Length)], transform.position, transform.rotation);
            client.transform.SetParent(canvas.transform, true);
            client.GetComponent<Customer>().canvas = canvas;
            client.GetComponent<Customer>().DeletePlace = DeletePlace;
            CustomerNow = client;
        }
        if(DeliveryManNow == null)
        {
            DeliveryManNow = Instantiate(DeliveryMan, transform.position, transform.rotation);
            DeliveryManNow.transform.SetParent(canvas.transform, true);
            DeliveryManNow.GetComponent<DeliveryMan>().BuyObjCount = Random.Range(2, 5);
            DeliveryManNow.GetComponent<DeliveryMan>().place = DeliveryPlace.transform;
            DeliveryManNow.GetComponent<DeliveryMan>().delete = DeletePlace;
        }
    }

    private IEnumerator CreateCar()
    {
        if (CountOfCars < 5)
        {
            GameObject car = Instantiate(Car, CarPlace.transform.position, Car.transform.rotation);
            car.transform.SetParent(canvas.transform, true);
            car.GetComponent<CarCustomer>().meneger = this;
            car.GetComponent<CarCustomer>().station = CarSellStation;
            CountOfCars++;
        }
        yield return new WaitForSeconds(3);
        StartCoroutine(CreateCar());
    }

    private IEnumerator CreateCustomer()
    {
        if(CustomerNow == null)
        {
            GameObject client = Instantiate(Customers[Random.Range(0, Customers.Length)], transform.position, transform.rotation);
            client.transform.SetParent(canvas.transform, true);
            client.GetComponent<Customer>().canvas = canvas;
            yield return new WaitForSeconds(5);
        }
    }
}
