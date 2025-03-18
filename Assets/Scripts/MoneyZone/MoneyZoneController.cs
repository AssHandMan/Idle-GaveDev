using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

//Special for Tima для работы в других стешенов
//1.obj с этим скриптом размещай как надо
//2. в строчке где ты писал писал wallet.ChangeCount(5, "Money") меняй на .GetComponent<MoneyZoneController>().Add();
//3. цена теперь будет тут _PriceOfCash
public class MoneyZoneController : MonoBehaviour
{
    public GameObject _model;
    [SerializeField] private int _PriceOfCash;
    [SerializeField] private int _realCountCash = 0;
    private PlayerWallet wallet;
    public List<GameObject> _cash3D = new List<GameObject>();
    public List<GameObject> _Listlayer;
    List<GameObject> _Pointlist = new List<GameObject>();
    [SerializeField] private int _testMaxCash_126; //Для проверки максимального количества отображаемых деняг(126)

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Start()
    {
        wallet = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerWallet>();
        foreach (GameObject p in _Listlayer)
        {
            GetChildRecursive(p);
        }
        for (int i = 0; i < _testMaxCash_126; i++) Add();
        
    }
    private void GetChildRecursive(GameObject obj)
    {
        if (null == obj)
            return;

        foreach (Transform child in obj.transform)
        {
            if (null == child)
                continue;
            //child.gameobject contains the current child you can do whatever you want like add it to an array
            _Pointlist.Add(child.gameObject);
            GetChildRecursive(child.gameObject);
        }
    }
    public void Add()//пополнение денег в куче
    {
        if (_realCountCash < 126)
        {
            for(int i =0; i < _PriceOfCash/5;i++)
            _cash3D.Add(Instantiate(_model, _Pointlist[_realCountCash].transform.position, _model.transform.rotation));
        }
        _realCountCash++;
       
        
    }
    public void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player" && _cash3D.Count>0)
        {
            StartCoroutine(DelCash());
        }
    }
    private IEnumerator DelCash()
    {
        yield return new WaitForSeconds(1);
        for (int i = 0; i < _cash3D.Count; i++)
        {
            wallet.ChangeCount(_PriceOfCash, "Money");
            Destroy(_cash3D[i]);
            _cash3D.RemoveAt(i);
            _realCountCash--;
        }
    }
}
