using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public float OffsetX, OffsetY, OffsetZ;
    public float L, R, U, D;
    private GameObject plr;
    private Vector3 target;

    void Start()
    {
        plr = GameObject.FindGameObjectWithTag("Player");

        //(plr.transform.position.x - 7.6f, plr.transform.position.y + 12.15f, plr.transform.position.z - 13.3f)
    }

    void Update()
    {
        target = new Vector3(plr.transform.position.x - OffsetX, plr.transform.position.y + OffsetY, plr.transform.position.z - OffsetZ);

        if (plr != null)
        {
            if (plr.transform.position.x > L && plr.transform.position.x < R && plr.transform.position.z > D && plr.transform.position.z < U)
            {
                transform.position = new Vector3(target.x, target.y, target.z);
            }
        }

    }
}
