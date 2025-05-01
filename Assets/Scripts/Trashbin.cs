using UnityEngine;
using static System.Collections.Specialized.BitVector32;

public class Trashbin : MonoBehaviour
{
    public Transform MenegerPlace;
    public bool inProgress;
    private Vector3 startPos;
    private Vector3 endPos;
    private Vector3 controlPoint;
    private float arcT = 0f;
    private float TakeSpeed;
    private GameObject LustTakenObj;
    void Start()
    {
        
    }

    void Update()
    {
        if (inProgress)
        {
            arcT += Time.deltaTime * TakeSpeed;
            arcT = Mathf.Clamp01(arcT);

            Vector3 pos = Mathf.Pow(1 - arcT, 2) * startPos + 2 * (1 - arcT) * arcT * controlPoint + Mathf.Pow(arcT, 2) * endPos;
            LustTakenObj.transform.position = pos;

            if (arcT >= 1f)
            {
                Destroy(LustTakenObj);
                inProgress = false;
            }
        }
    }

    public void AddObj(GameObject obj, float speed)
    {
        if (!inProgress)
        {
            TakeSpeed = speed;
            obj.transform.parent = null;
            startPos = obj.transform.position;
            arcT = 0;
            endPos = transform.position;
            controlPoint = (startPos + endPos) / 2f;
            controlPoint.y += 3f;

            LustTakenObj = obj;
            inProgress = true;
        }
    }
}
