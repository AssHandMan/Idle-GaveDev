using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object : MonoBehaviour
{
    public string Type;
    public Transform UpPos, DownPos;
    private float trailTrue = 0.2f;
    public List<GameObject> _models = new List<GameObject>();

    public void Start()
    {
        if(Type == "Trash")
            Instantiate(_models[Random.Range(0,_models.Count)],transform);
    }
    public void ChangeTrail()
    {
        TrailRenderer trail = GetComponent<TrailRenderer>();
        if(trail.time == trailTrue) { trail.time = 0; }
        else { trail.time = trailTrue; }
    }
}
