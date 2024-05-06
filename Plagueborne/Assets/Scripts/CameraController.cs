using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class CameraController : MonoBehaviour
{
    private Transform _target;
    public float deadZoneRadius;
    public float lerpTuning = 0.5f;
    public float speed;
    void Start()
    {
        _target = GameObject.FindGameObjectWithTag("Player").transform;
    }
    void Update()
    {
        if (Vector2.Distance(transform.position, _target.position) > deadZoneRadius)
        {
            Vector2 distance = Vector2.Lerp(transform.position, _target.position, lerpTuning) 
                - (Vector2)transform.position;
            transform.Translate(distance * speed * Time.deltaTime, Space.World);
        }
    }
}
