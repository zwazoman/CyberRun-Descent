using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [HideInInspector] public float MaxX;
    [HideInInspector] public float SmoothTime;
    [HideInInspector] public int ShotsNumber;
    [HideInInspector] public float TimeBetweenShots;
    [HideInInspector] public float MoveSpeed;

    [SerializeField] GameObject _bullet;

    Vector3 _targetSpot;

    bool _isLeaving;
    Vector3 chien;
    bool _hasShot;

    void Start()
    {
        _targetSpot = new Vector3 (MaxX,transform.position.y,transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.SmoothDamp(transform.position, _targetSpot, ref chien, SmoothTime);
        Vector3 distance = transform.position - _targetSpot;
        if( distance.sqrMagnitude <= 1.5 * 1.5 && !_hasShot)
        {
            StartCoroutine(StartShooting());
            _hasShot = true;
        }
        if(_isLeaving) transform.position += Vector3.left * MoveSpeed * Time.deltaTime;
    }

    IEnumerator StartShooting()
    {
        int cpt = 0;
        while (cpt < ShotsNumber)
        {
            yield return new WaitForSeconds(TimeBetweenShots);
            Shoot();
            cpt += 1;
        }
        yield return new WaitForSeconds(TimeBetweenShots);
        Leave();
    }

    void Shoot()
    {
        Instantiate(_bullet, transform.position, Quaternion.identity);
    }

    void Leave()
    {
        _isLeaving = true;
    }

    public void Die()
    {
        //juice
        Destroy(gameObject);
    }
}
