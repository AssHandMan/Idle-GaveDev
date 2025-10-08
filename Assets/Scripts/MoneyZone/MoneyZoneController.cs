
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;

//Special for Tima                             
//1.obj                                  
//2.                              wallet.ChangeCount(5, "Money")          .GetComponent<MoneyZoneController>().Add();
//3.                       _PriceOfCash
public class MoneyZoneController : MonoBehaviour
{
    [SerializeField] int[,,] massiv;
    public GameObject _model;
    [SerializeField] private int _PriceOfCash;
    [SerializeField] private int _PriceCoeff;
    [SerializeField] private int _realCountCash = 0;
    private PlayerWallet wallet;
    public List<GameObject> _cash3D = new List<GameObject>();
    public List<GameObject> _Listlayer;
    List<GameObject> _Pointlist = new List<GameObject>();
    [SerializeField] private int _testMaxCash; //                                                        

    [SerializeField] private int _X_SizeStack;
    [SerializeField] private int _Y_SizeStack;
    [SerializeField] private int _Z_SizeStack;
    [SerializeField] private float _X_Spacing;
    [SerializeField] private float _Y_Spacing;
    [SerializeField] private float _Z_Spacing;
    [SerializeField] public GameObject _FirstPoint;

    [SerializeField] private GameObject _MoneyEffectObj;
    private Coroutine moneyEffectCoroutine;

    public float removalSpeed = 1.0f; // 1.0 -                    , >1 -          , <1 -           
    private bool isRemoving = false; //     ,                                           
    private Coroutine removalCoroutine; //                              
    [Header("Audio")]
    public AudioSource _audio;
    public AudioClip _clip1, _clip2;
    public float _volume = 0.4f;
    [SerializeField] private int maxCoord;
    [SerializeField] private int coord = 0;


    public void Start()
    {
        massiv = new int[_Z_SizeStack, _X_SizeStack, _Y_SizeStack];
        wallet = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerWallet>();
        maxCoord = _X_SizeStack + _Y_SizeStack + _Z_SizeStack;
        //StackSize();
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
    public void Add()//                       
    {
        if (_realCountCash < (_X_SizeStack * _Y_SizeStack * _Z_SizeStack))
        {
            for (int i = 0; i < _PriceOfCash / _PriceCoeff; i++)
            {
                //Debug.Log($"{_X_Spacing},{_Y_Spacing},{_Z_Spacing}");

                CheckStackSize(coord);
                coord++;
            }
        }
        _realCountCash += (_PriceOfCash / _PriceCoeff);


    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && _realCountCash > 0 && !isRemoving)
        {
            removalCoroutine = StartCoroutine(RemoveCashLayers());
            _audio.PlayOneShot(_clip2, _volume);
        }
        if (other.CompareTag("Player") && _cash3D.Count > 0)
        {
            if (moneyEffectCoroutine == null)
                moneyEffectCoroutine = StartCoroutine(MoneyEffectGenerator());
        }
    }
    private void CheckStackSize(int coord)
    {
        int temp = 0;
        for (int y = 0; y < _Y_SizeStack; y++)
        {
            for (int x = 0; x < _X_SizeStack; x++)
            {
                for (int z = 0; z < _Z_SizeStack; z++)
                {
                    if (temp == coord)
                    {
                        _cash3D.Add(Instantiate(_model, new Vector3(_FirstPoint.transform.position.x + (_X_Spacing * x), _FirstPoint.transform.position.y + (_Y_Spacing * y), _FirstPoint.transform.position.z + _Z_Spacing * z), _model.transform.rotation));
                        return;
                    }
                    temp++;
                }
            }
        }
    }
    private void StackSize()
    {

        for (int y = 0; y < _Y_SizeStack; y++)
        {
            for (int x = 0; x < _X_SizeStack; x++)
            {
                for (int z = 0; z < _Z_SizeStack; z++)
                {
                    //                                      
                    Vector3 position = new Vector3(_FirstPoint.transform.position.x + x * _X_Spacing,
                        _FirstPoint.transform.position.y + y * _Y_Spacing,
                        _FirstPoint.transform.position.z + z * _Z_Spacing);

                    _Pointlist.Add(Instantiate(_FirstPoint, position, _model.transform.rotation));

                }
            }
        }
    }
    private IEnumerator RemoveCashLayers()
    {
        isRemoving = true;

        //           ,                            (                             )
        int billsPerLayer = Mathf.Max(1, _cash3D.Count / _realCountCash);

        while (_realCountCash > 0)
        {
            //                   (             ,           )
            int startIndex = Mathf.Max(0, _cash3D.Count - billsPerLayer);
            int countToRemove = Mathf.Min(billsPerLayer, _cash3D.Count);
            //Debug.Log(countToRemove);
            for (int i = startIndex; i < _realCountCash; i++)
            {
                if (i < startIndex + countToRemove)
                {
                    Destroy(_cash3D[i]);
                }
                _realCountCash--;
                wallet.ChangeCount(_PriceOfCash, "Money");
            }

            _audio.PlayOneShot(_clip1, _volume);
            _cash3D.RemoveRange(startIndex, countToRemove);
            coord = 0;

            //                                     (                   )
            yield return new WaitForSeconds(1f / removalSpeed);
        }
        
        isRemoving = false;
        moneyEffectCoroutine = null;
        StopCoroutine(MoneyEffectGenerator());
    }
    private IEnumerator MoneyEffectGenerator()
    {
        while (_cash3D.Count > 0)
        {
            GameObject money = Instantiate(_MoneyEffectObj, transform.position, _MoneyEffectObj.transform.rotation);
            money.GetComponent<MoneyBuySpace>().TakeMoney();
            yield return new WaitForSeconds(0.1f);
        }

        moneyEffectCoroutine = null;
    }
}
