using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] float _laserTime = 1;

    Vector3 chien;

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.SmoothDamp(transform.position, Player.Instance.transform.position,ref chien, _laserTime);
    }
}
