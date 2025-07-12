using UnityEngine;

public class WorkerAnim : MonoBehaviour
{
    [SerializeField] private Animator animl;

    [SerializeField] private SellStation Cash;
    [SerializeField] private CarSellStation Car;
    [SerializeField] private WorkStation Work;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Cash != null)
        {
            if(Cash.station.CountObj > 0 & Cash.client != null)
            {
                animl.SetBool("IsPlay", true);
            }
            else
            {
                animl.SetBool("IsPlay", false);
            }
        }
        else if(Car != null)
        {
            if (Car.station.CountObj > 0 & Car.client != null)
            {
                animl.SetBool("IsPlay", true);
            }
            else
            {
                animl.SetBool("IsPlay", false);
            }
        }
        else 
        { 
            if(Work.station.CountObj > 0)
            {
                animl.SetBool("IsPlay", true);
            }
            else
            {
                animl.SetBool("IsPlay", false);
            }
        }
    }
}
