using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.PlayerSettings;

public class ObjTest : MonoBehaviour
{
    public bool inProgress;
    public float speed;
    public Transform UpPos, DownPos;
    public GameObject hands;
    public PlayerTakeDropSystem _hands;
    void Start()
    {
        
    }

    
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Z))
        {
            Vector3 pos;
            if (_hands.Count == 0) { pos = hands.transform.position; }
            else { pos = _hands.IngredientsCollection[_hands.IngredientsCollection.Count - 1].GetComponent<Object>().UpPos.position - DownPos.localPosition; }
            _hands.IngredientsCollection.Add(this.gameObject);
            _hands.Count++;
            StartCoroutine(Moving(pos, speed));
            inProgress = true;
        }
    }

    private IEnumerator Moving(Vector3 newPos, float speed)
    {
        transform.position = Vector3.MoveTowards(transform.position, hands.transform.position, speed);
        yield return null;
        if(transform.position != newPos)
        {
            StartCoroutine(Moving(newPos, speed));
        }
        else
        {
            transform.parent = hands.transform;
            inProgress = false;
        }
    }
}
