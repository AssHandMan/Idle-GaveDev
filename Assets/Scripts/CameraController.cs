using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public float OffsetX, OffsetY, OffsetZ;
    public Vector3 PlatRotate, SkinRotate, SkinPos;
    public bool isPlay;
    private GameObject plr;
    private Vector3 target;

    void Start()
    {
        isPlay = true;
        plr = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if(isPlay)
        {
            target = new Vector3(plr.transform.position.x - OffsetX, plr.transform.position.y + OffsetY, plr.transform.position.z - OffsetZ);
            if (plr != null)
            {
                transform.position = new Vector3(target.x, target.y, target.z);
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

}
