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

    [SerializeField] AudioClip[] _groundDiveSound;
    [SerializeField] float _groundDiveSoundVolume;

    [SerializeField] AudioClip[] _groundDiveSound2;
    [SerializeField] float _groundDiveSoundVolume2;

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

            if (!isGrounded)
            {
                Player.Instance.RB.velocity = Vector3.zero;
                if (Player.Instance.IsDiving)
                {
                    OnGroundDiveHit?.Invoke();
                    //SFXManager.Instance.PlaySFXClip(_groundDiveSound,transform.position,_groundDiveSoundVolume);
                    SFXManager.Instance.PlaySFXClip(_groundDiveSound2,transform.position,_groundDiveSoundVolume2);

                    if (hit.collider.gameObject.TryGetComponent<DestroyableThing>(out DestroyableThing thing))
                    {
                        thing.hit();
                        Player.Instance.Jump();
                        if(Input.GetKey(KeyCode.Space)) Player.Instance.Jump();//t'inquiete
                    }


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
