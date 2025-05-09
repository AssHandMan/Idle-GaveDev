using UnityEngine;
using UnityEngine.UIElements;


public class MoneyBuySpace : MonoBehaviour
{
    [SerializeField] float Speed;
    private bool inGiving, inTaking;
    private int CountForLustObj;

    private Vector3 startPos;
    private Vector3 endPos;
    private Vector3 controlPoint;
    private Transform plr;
    private float arcT = 0f;
    void Start()
    {
        
    }


    void Update()
    {
        if (inGiving)
        {
            arcT += Time.deltaTime * Speed;
            arcT = Mathf.Clamp01(arcT);

            controlPoint = (startPos + endPos) / 2f;
            controlPoint.y += 7f;

            Vector3 pos = Mathf.Pow(1 - arcT, 2) * startPos + 2 * (1 - arcT) * arcT * controlPoint + Mathf.Pow(arcT, 2) * endPos;
            transform.position = pos;

            if (arcT >= 1f)
            {
                Destroy(gameObject);
            }
        }

        if (inTaking)
        {
            arcT += Time.deltaTime * Speed;
            arcT = Mathf.Clamp01(arcT);

            endPos = plr.position;

            controlPoint = (startPos + endPos) / 2f;
            controlPoint.y += 3f;

            Vector3 pos = Mathf.Pow(1 - arcT, 2) * startPos + 2 * (1 - arcT) * arcT * controlPoint + Mathf.Pow(arcT, 2) * endPos;
            transform.position = pos;

            if (arcT >= 1f)
            {
                Destroy(gameObject);
            }
        }
    }
    public void GiveMoney(Vector3 end)
    {
        startPos = transform.position;
        arcT = 0;
        endPos = new Vector3(end.x, end.y - 5, end.z);
        inGiving = true;
    }
    public void TakeMoney()
    {
        startPos = transform.position;
        arcT = 0f;
        plr = GameObject.FindGameObjectWithTag("Player").transform;
        inTaking = true;
    }
}
