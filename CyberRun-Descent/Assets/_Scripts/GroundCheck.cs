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

    bool isGrounded = false;

    private void Awake()
    {
        _coll = gameObject.GetComponent<Collider>();
    }

    private void Start()
    {
        GameManager.instance.OnGameOver += () => enabled = false;
    }

    private void FixedUpdate()
    {
        if (Physics.SphereCast(Player.Instance.RB.position+ Vector3.up*.5f, .5f, Vector3.down, out RaycastHit hit,Mathf.Max(-Player.Instance.RB.velocity.y*Time.deltaTime,.6f), LayerMask.GetMask("Limits")))
        {
            print("grounded ptn");
            if (!isGrounded)
            {

                if (Player.Instance.IsDiving)
                {
                    OnGroundDiveHit?.Invoke();
                }
                else
                {
                    OnGroundHit?.Invoke();
                }
                isGrounded = true;
            }

        }
        else if(isGrounded)
        {
            isGrounded = false;
            OnGroundLeave?.Invoke();
        }
    }

   /* private void OnTriggerEnter(Collider other)
    {
        if (Player.Instance.IsDiving)
        {
            if (other.gameObject.layer == 6) OnGroundDiveHit?.Invoke();
        }
        else
        {
            if (other.gameObject.layer == 6) OnGroundHit?.Invoke();
        }
    }*/

   /* private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 6) OnGroundLeave?.Invoke();
    }*/
}
