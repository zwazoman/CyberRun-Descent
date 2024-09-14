using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public event Action OnGroundHit;
    public event Action OnGroundLeave;

    Collider _coll;

    private void Awake()
    {
        _coll = gameObject.GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        OnGroundHit?.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        OnGroundLeave?.Invoke();
    }
}
