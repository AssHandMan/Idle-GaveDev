using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerGenerator : MonoBehaviour
{
    public Canvas canvas;
    public GameObject[] Customers;
    
    public GameObject Car;
    public int CountOfCars;
    public CarSellStation CarSellStation;
    public Transform CarPlace;

    private GameObject CustomerNow;

    void Start()
    {
        StartCoroutine(CreateCar());
        //StartCoroutine(CreateCustomer());
    }

    // Update is called once per frame
    void Update()
    {
        if (CustomerNow == null)
        {
            GameObject client = Instantiate(Customers[Random.Range(0, Customers.Length)], transform.position, transform.rotation);
            client.transform.SetParent(canvas.transform, true);
            client.GetComponent<Customer>().canvas = canvas;
            CustomerNow = client;
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
