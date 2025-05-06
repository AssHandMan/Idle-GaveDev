using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object : MonoBehaviour
{
    public string Type;
    public Transform UpPos, DownPos;
    private float trailTrue = 0.2f;

    public void ChangeTrail()
    {
        TrailRenderer trail = GetComponent<TrailRenderer>();
        if(trail.time == trailTrue) { trail.time = 0; }
        else { trail.time = trailTrue; }
    }
}
