using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platforms : MonoBehaviour
{
    [SerializeField] float _moveSpeed;

    private void Start()
    {
        _moveSpeed *= GameManager.Difficulty;
    }

    private void Update()
    {
        transform.position += Vector3.left * _moveSpeed * Time.deltaTime;
        if(transform.position.x<-100)Destroy(gameObject);
    }
}
