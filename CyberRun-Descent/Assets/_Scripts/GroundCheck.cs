using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class GroundCheck : MonoBehaviour
{
    public UnityEvent OnGroundHit;
    public UnityEvent OnGroundDiveHit;
    public UnityEvent OnGroundLeave;

    Collider _coll;

    private void Awake()
    {
        _coll = gameObject.GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Player.Instance.IsDiving)
        {
            if (other.gameObject.layer == 6) OnGroundDiveHit?.Invoke();
        }
        else
        {
            if (other.gameObject.layer == 6) OnGroundHit?.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 6) OnGroundLeave?.Invoke();
    }
}
