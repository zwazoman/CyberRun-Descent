using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float _moveSpeed;

    Vector3 _targetPos;

    void Start()
    {
        _targetPos = Player.Instance.transform.position;
        transform.LookAt( _targetPos );
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * _moveSpeed * Time.deltaTime;
    }
}
