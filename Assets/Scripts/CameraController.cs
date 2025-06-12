using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public float OffsetX, OffsetY, OffsetZ, FixX, FixZ;
    public Vector3 PlatRotate, SkinRotate, SkinPos;
    public bool isPlay;
    private GameObject plr;
    private Vector3 target;

    void Start()
    {
        isPlay = true;
        plr = GameObject.FindGameObjectWithTag("Player");
    }

    void LateUpdate()
    {
        target = new Vector3(plr.transform.position.x - OffsetX, plr.transform.position.y + OffsetY, plr.transform.position.z - OffsetZ);
        if(isPlay)
        {
            if (plr != null)
            {
                if(plr.transform.position.x > FixX)
                {
                    transform.position = new Vector3(transform.position.x, target.y, target.z);
                }
                else if(plr.transform.position.z < FixZ)
                {
                    transform.position = new Vector3(target.x, target.y, transform.position.z);
                }
                else 
                {
                    transform.position = new Vector3(target.x, target.y, target.z);   
                }
            }
        }
    }

    public void SetPlayMood()
    {
        isPlay=true;
        transform.rotation = Quaternion.Euler(PlatRotate);
    }
    public void SetSkinMood()
    {
        isPlay = false;
        transform.position = SkinPos;
        transform.rotation = Quaternion.Euler(SkinRotate);
    }
    public void SetBuySpacePos(GameObject space)
    {
        if (isPlay)
        {
            isPlay = false;
            Vector3 targetBuy = new Vector3(space.transform.position.x - OffsetX, transform.position.y, space.transform.position.z - OffsetZ);
            StartCoroutine(MoveToLustBuytSpace(targetBuy));
        }
    }

    private IEnumerator MoveToLustBuytSpace(Vector3 pos)
    {
        while(Vector3.Distance(transform.position, pos) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, pos, 17 * Time.deltaTime);
            yield return null;
        }
        yield return new WaitForSeconds(0.4f);
        while (Vector3.Distance(transform.position, target) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, 17 * Time.deltaTime);
            yield return null;
        }
        isPlay = true;
    }
}
