using UnityEngine;

public class ChitMoney : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Hands")
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerWallet>().ChangeCount(100000, "Money");
        }
    }
}
