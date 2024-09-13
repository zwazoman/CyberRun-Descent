using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    Collider _coll;

    private void Awake()
    {
        _coll = gameObject.GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerControls.Instance.IsGrounded = true;
        if (PlayerControls.Instance.IsDiving)
        {
            //con de dive
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerControls.Instance.IsGrounded = false;
    }
}
