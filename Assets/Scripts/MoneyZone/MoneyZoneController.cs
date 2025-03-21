using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

//Special for Tima дл€ работы в других стешенов
//1.obj с этим скриптом размещай как надо
//2. в строчке где ты писал писал wallet.ChangeCount(5, "Money") мен€й на .GetComponent<MoneyZoneController>().Add();
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
    [SerializeField] private int _testMaxCash; //ƒл€ проверки максимального количества отображаемых ден€г

    [SerializeField] private int _X_SizeStack;
    [SerializeField] private int _Y_SizeStack;
    [SerializeField] private int _Z_SizeStack;
    [SerializeField] private float _X_Spacing;
    [SerializeField] private float _Y_Spacing;
    [SerializeField] private float _Z_Spacing;
    [SerializeField] public GameObject _FirstPoint;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Start()
    {
        wallet = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerWallet>();
        StackSize();
        /*foreach (GameObject p in _Listlayer)
        {
            GetChildRecursive(p);
        }*/
        for (int i = 0; i < _testMaxCash; i++) Add();
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
        if (_realCountCash < (_X_SizeStack* _Y_SizeStack* _Z_SizeStack))
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
            for (int i = 0; i < _realCountCash+1; i++)
            {
                wallet.ChangeCount(_PriceOfCash, "Money");
                if (_cash3D.ElementAtOrDefault(i) != null)
                {
                    Destroy(_cash3D[i]);
                    _cash3D.RemoveAt(i);
                }
                _realCountCash--;
            }
        }
    }
    private void StackSize()
    {

        for (int x = 0; x < _X_SizeStack; x++)
        {
            for (int y = 0; y < _Y_SizeStack; y++)
            {
                for (int z = 0; z < _Z_SizeStack; z++)
                {
                    // ¬ычисл€ем позицию дл€ каждого объекта
                    Vector3 position = new Vector3(_FirstPoint.transform.position.x + x * _X_Spacing,
                        _FirstPoint.transform.position.y + y * _Y_Spacing,
                        _FirstPoint.transform.position.z + z * _Z_Spacing);

                    _Pointlist.Add(Instantiate(_FirstPoint, position, _model.transform.rotation));
                    
                }
            }
        }
        }
   /* private IEnumerator DelCash()
    {
        yield return new WaitForSeconds(1);
        
    }*/
}
