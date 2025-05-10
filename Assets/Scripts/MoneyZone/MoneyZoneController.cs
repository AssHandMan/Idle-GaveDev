
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//Special for Tima для работы в других стешенов
//1.obj с этим скриптом размещай как надо
//2. в строчке где ты писал писал wallet.ChangeCount(5, "Money") меняй на .GetComponent<MoneyZoneController>().Add();
//3. цена теперь будет тут _PriceOfCash
public class MoneyZoneController : MonoBehaviour
{
    public GameObject _model;
    [SerializeField] private int _PriceOfCash;
    [SerializeField] private int _PriceCoeff;
    [SerializeField] private int _realCountCash = 0;
    private PlayerWallet wallet;
    public List<GameObject> _cash3D = new List<GameObject>();
    public List<GameObject> _Listlayer;
    List<GameObject> _Pointlist = new List<GameObject>();
    [SerializeField] private int _testMaxCash; //Для проверки максимального количества отображаемых деняг

    [SerializeField] private int _X_SizeStack;
    [SerializeField] private int _Y_SizeStack;
    [SerializeField] private int _Z_SizeStack;
    [SerializeField] private float _X_Spacing;
    [SerializeField] private float _Y_Spacing;
    [SerializeField] private float _Z_Spacing;
    [SerializeField] public GameObject _FirstPoint;

    [SerializeField] private GameObject _MoneyEffectObj;
    private Coroutine moneyEffectCoroutine;

    public float removalSpeed = 1.0f; // 1.0 - нормальная скорость, >1 - ускорение, <1 - замедление
    private bool isRemoving = false; // Флаг, чтобы не запускать несколько корутин сразу
    private Coroutine removalCoroutine; // Ссылка на запущенную корутину

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
            for(int i =0; i < _PriceOfCash/_PriceCoeff;i++)
            _cash3D.Add(Instantiate(_model, _Pointlist[_realCountCash].transform.position, _model.transform.rotation));
        }
        _realCountCash+=(_PriceOfCash / _PriceCoeff);
       
        
    }
    public void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && _cash3D.Count > 0 && !isRemoving)
        {
            removalCoroutine = StartCoroutine(RemoveCashLayers());
        }
    }

    // При выходе из триггера
    public void OnTriggerExit(Collider other)
    {

    }
    private void StackSize()
    {

        for (int y = 0; y < _Y_SizeStack; y++)
        {
            for (int x = 0; x < _X_SizeStack; x++)
            {
                for (int z = 0; z < _Z_SizeStack; z++)
                {
                    // Вычисляем позицию для каждого объекта
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

        // Определяем, сколько купюр в одном слое (если у вас слоистая структура)
        int billsPerLayer = Mathf.Max(1, _cash3D.Count / _realCountCash);

        while (_cash3D.Count > 0)
        {
            // Удаляем один слой (или несколько, если нужно)
            int startIndex = Mathf.Max(0, _cash3D.Count - billsPerLayer);
            int countToRemove = Mathf.Min(billsPerLayer, _cash3D.Count);

            for (int i = startIndex; i < startIndex + countToRemove; i++)
            {
                if (_cash3D[i] != null)
                {
                    wallet.ChangeCount(_PriceOfCash, "Money");
                    Destroy(_cash3D[i]);
                }
            }

            _cash3D.RemoveRange(startIndex, countToRemove);
            _realCountCash -= countToRemove;

            // Ждём перед удалением следующего слоя (регулируем скорость)
            yield return new WaitForSeconds(1f / removalSpeed);
        }

        isRemoving = false;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && _cash3D.Count > 0)
        {
            if (moneyEffectCoroutine == null)
                moneyEffectCoroutine = StartCoroutine(MoneyEffectGenerator());
        }
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
