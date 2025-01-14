using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarCustomer : MonoBehaviour
{
    public CustomerGenerator meneger;
    public CarSellStation station;
    public Text CountObjImg;
    public Image OrderBoard;
    public float speed;
    public int BuyObjCount;


    private float MaxSpeed;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        MaxSpeed = speed;
        
        BuyObjCount = Random.Range(4, 8);
        CountObjImg.text = BuyObjCount.ToString();
    }

    // Update is called once per frame
    void Update()
    {

        rb.velocity = new Vector3(0, 0, speed);
        if (OrderBoard != null) { OrderBoard.transform.LookAt(Camera.main.transform.position); }
        if (transform.position.z < station.transform.position.z + 3)
        {
            if (BuyObjCount != 0)
            {
                speed = 0;
                station.client = this;
            }
            else
            {
                speed = MaxSpeed;
            }
        }
        if(transform.position.z < -20)
        {
            meneger.CountOfCars--;
            Destroy(gameObject);
        }
    }

    public void ChangeCountBuy()
    {
        BuyObjCount--;
        CountObjImg.text = BuyObjCount.ToString();
        if (BuyObjCount == 0)
        {
            Destroy(OrderBoard);
            Destroy(CountObjImg);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<CarCustomer>() != null)
        {
            speed = 0;
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<CarCustomer>() != null)
        {
            speed = MaxSpeed;
        }
    }
}
