using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platforms : MonoBehaviour
{
    [SerializeField] float _moveSpeed;

    private void Update()
    {
        transform.position += Vector3.left * _moveSpeed * Time.deltaTime;
    }
}
