using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class DeliveryMan : MonoBehaviour
{
    public Transform place, delete, target;
    public int BuyObjCount;
    public GameObject OrderBar;
    public Text OrderCount;
    public string Type;
    private NavMeshAgent agent;
    private PlayerWallet wallet;
    private int MaxBuyObjCount;

    [SerializeField] private Animator _animator;

    void Start()
    {
        wallet = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerWallet>();
        agent = GetComponent<NavMeshAgent>();
        OrderBar.SetActive(false);
        MaxBuyObjCount = BuyObjCount;
        OrderCount.text = (MaxBuyObjCount - BuyObjCount).ToString() + "/" + MaxBuyObjCount.ToString();
        target = place;
        _animator.SetBool("IsMoving", true);

    }

    // Update is called once per frame
    void Update()
    {

        agent.SetDestination(target.position);
        if (transform.position.x == target.position.x && transform.position.z == target.position.z) 
        {
            if (!OrderBar.activeSelf) { OrderBar.SetActive(true); } 
            if (BuyObjCount > 0) { OrderBar.transform.LookAt(Camera.main.transform); _animator.SetBool("IsMoving", false); }

        }
        else
        {
            _animator.SetBool("IsMoving", true);
        }

        if (transform.position.x == delete.position.x && transform.position.z == delete.position.z) { Destroy(gameObject); }
    }

    public void AddToOrder()
    {
        if (BuyObjCount > 0)
        {
            BuyObjCount--;
            wallet.ChangeCount(10, "Money");
            OrderCount.text = (MaxBuyObjCount - BuyObjCount).ToString() + "/" + MaxBuyObjCount.ToString();
        }
        if (BuyObjCount == 0)
        {
            OrderBar.SetActive(false);
            target = delete;
        }
    }
}
