using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    public bool IsGrounded { get; set; }
    public bool IsDiving { get; private set; }

    [SerializeField] float _jumpForce = 1;
    [SerializeField] float _floatForce = 1;
    [SerializeField] float _diveForce = 1;

    bool _isJumping;
    bool _stopFloating = false;
    Rigidbody _rb;
    

    private static PlayerControls instance = null;
    public static PlayerControls Instance => instance;
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

    void Update()
    {
        if (_isJumping) 
        {
            _rb.AddForce(Vector3.up * _floatForce);
        }
    }

    public void OnSpaceBar(InputAction.CallbackContext context)
    {
        if (context.performed) 
        {
            if (IsGrounded)
            {
                print("Jump");
                Jump();
                _isJumping = true; 
            }
            else
            {
                print("Dive");
                Dive();
            }
        }
        if (context.canceled)
        {
            _isJumping = false;
        }
    }

    void Jump()
    {
        _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
    }
    
    void Dive()
    {
        _rb.AddForce(Vector3.down * _diveForce);
        IsDiving = true;
    }
}
