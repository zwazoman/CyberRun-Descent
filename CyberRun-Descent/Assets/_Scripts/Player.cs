using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public bool IsGrounded { get; private set; }
    public bool IsJumping { get; private set; }
    public bool IsDiving { get; private set; }
    public bool IsSuspended { get; private set; }

    public event Action OnJump;
    public event Action OnSuspend;
    public event Action OnDive;

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
        _groundCheck.OnGroundHit += HitsGround;
        _groundCheck.OnGroundLeave += LeavesGround;
    }

    void Update()
    {
        if (IsJumping) if (_rb.velocity.y >= 0) _rb.AddForce(Vector3.up * _jumpBoostForce);
        if (IsSuspended) _rb.AddForce(Vector3.up * _suspendedForce);
    }

    public void OnSpaceBar(InputAction.CallbackContext context)
    {
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
                if (!IsDiving) Suspend();
            }
        }
        if (context.canceled)
        {
            IsJumping = false;
            if(IsSuspended) Dive();
        }
    }

    void Jump()
    {
        _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
    }
    
    void Suspend()
    {
        OnSuspend?.Invoke();
        _rb.velocity = Vector3.zero;
        //_rb.useGravity = false;
        IsSuspended = true;
    }

    void Dive()
    {
        //_rb.useGravity = true;
        IsSuspended = false;
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
}
