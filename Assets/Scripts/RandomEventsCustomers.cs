using System.Collections;
using UnityEngine;

public class RandomEventsCustomers : MonoBehaviour
{
    public SellStation station;
    public float timeEventInterval = 60;

    void Start()
    {
        StartCoroutine(MakeCall());
    }


    void Update()
    {

    }


    private IEnumerator MakeCall()
    {
        yield return new WaitForSeconds(timeEventInterval);
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
                        }
                    }
                }
            }
        }
        StartCoroutine(MakeCall());
    }
}
