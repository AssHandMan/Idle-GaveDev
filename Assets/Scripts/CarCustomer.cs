using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;

public class CarCustomer : MonoBehaviour
{
    public CustomersMeneger meneger;
    public CarSellStation station;
    public Text CountObjImg;
    public Image OrderBoard;
    public float speed;
    public int BuyObjCount;


    private float MaxSpeed;
    private Rigidbody rb;
    [SerializeField] private List<GameObject> _models = new List<GameObject>();
    public GameObject model;
    void Start()
    {
        SelectModel();
        rb = GetComponent<Rigidbody>();
        MaxSpeed = speed;
        CountObjImg.text = BuyObjCount.ToString();
    }

    // Update is called once per frame
    void Update()
    {

        rb.linearVelocity = new Vector3(0, 0, speed);
        if (OrderBoard != null) { OrderBoard.transform.LookAt(Camera.main.transform.position); }
        if (transform.position.z < station.transform.position.z-2)
        {
            if (BuyObjCount != 0)
            {
                speed = 0;
                station.client = this;
            }
            else
            {
                if(speed != MaxSpeed*2) { speed = MaxSpeed * 2; }
            }
        }
        if(transform.position.z < -20)
        {
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
            meneger.CountOfCars--;
        }
    }
    private void SelectModel()
    {
        Instantiate(_models[Random.Range(0, _models.Count)],model.transform);
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<CarCustomer>() != null || other.GetComponent<Customer>() != null)
        {
            if (other.gameObject.transform.position.z < transform.position.z)
            { speed = 0; }
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<CarCustomer>() != null || other.GetComponent<Customer>() != null)
        {
            speed = MaxSpeed;
        }
    }
}
