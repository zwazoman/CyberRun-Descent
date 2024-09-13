using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demo_Rotation : MonoBehaviour
{
    [SerializeField] float _radius;
    [SerializeField] float _speedMultiplier;
    [SerializeField] Transform _target;

    // Update is called once per frame
    void Update()
    {
        _target.position = transform.position + _radius * new Vector3(Mathf.Sin(Time.time*_speedMultiplier),0, Mathf.Cos(Time.time * _speedMultiplier));
    }
}
