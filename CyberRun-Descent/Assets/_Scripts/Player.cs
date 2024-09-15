using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public bool IsGrounded { get; private set; }
    public bool IsJumping { get; private set; }
    public bool IsDiving { get; private set; }
    public bool IsSuspended { get; private set; }

    public UnityEvent OnJump;
    public UnityEvent OnSuspend;
    public UnityEvent OnDive;

    [Header("Parameters")]
    [SerializeField] float _jumpForce = 1;
    [SerializeField] float _jumpBoostForce = 1;
    [SerializeField] float _diveForce = 1;
    [SerializeField] float _suspendedForce = 2;

    [Header("References")]
    [SerializeField] GroundCheck _groundCheck;


    Rigidbody _rb;
    

    private static Player instance = null;
    public static Player Instance => instance;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }

        _rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        GameManager.instance.OnGameOver += () => this.enabled = false;
        _groundCheck.OnGroundHit.AddListener(HitsGround);
        _groundCheck.OnGroundLeave.AddListener(LeavesGround);
    }

    void Update()
    {
        if (IsJumping) if (_rb.velocity.y >= 0) _rb.AddForce(Vector3.up * _jumpBoostForce);
        if (IsSuspended) _rb.AddForce(Vector3.up * _suspendedForce);
    }

    public void OnSpaceBar(InputAction.CallbackContext context)
    {
        if (!enabled) return;

        if (context.performed) 
        {
            if (IsGrounded)
            {
                OnJump?.Invoke();
                Jump();
                IsJumping = true; 
            }
            else
            {
                OnDive?.Invoke();
                if (!IsDiving) Dive();
            }
        }
        if (context.canceled)
        {
            IsJumping = false;
            //if(IsSuspended) Dive();
        }
    }

    void Jump()
    {
        if (!enabled) return;

        _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
    }
    
    void Suspend()
    {
        if (!enabled) return;

        OnSuspend?.Invoke();
        _rb.velocity = Vector3.zero;
        //_rb.useGravity = false;
        IsSuspended = true;
    }

    void Dive()
    {
        if (!enabled) return;

        //_rb.useGravity = true;
        IsSuspended = false;
        _rb.velocity = Vector3.zero;
        _rb.AddForce(Vector3.down * _diveForce, ForceMode.Impulse);
        IsDiving = true;
    }

    void HitsGround()
    {
        IsGrounded = true;
        IsDiving = false;
    }

    void LeavesGround()
    {
        IsGrounded = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "DENJEUREUX")
        {
            GetComponent<Collider>().enabled = false;

            Vector3 force = UnityEngine.Random.insideUnitSphere;
            force.y = Mathf.Abs(force.y) + 1f;
            force *= 10;

            _rb.velocity = Vector3.zero;
            _rb.AddForce(force, ForceMode.Impulse);

            GameManager.instance.TriggerGameOver();
        }
    }

}
