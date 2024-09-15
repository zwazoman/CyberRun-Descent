using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class Laser : MonoBehaviour
{
    [SerializeField] AudioClip[] _laserBeepSound;
    [SerializeField] float _laserBeepVolume;

    [SerializeField]AudioClip[] _laserShootSound1;
    [SerializeField] float _laserShootVolume1;

    [SerializeField] AudioClip[] _laserShootSound2;
    [SerializeField] float _laserShootVolume2;

    [SerializeField] float _laserTime = 1;

    laserVisuals _visuals;
    Collider _col;

    float LifetimeMultiplier = 1;
    Vector3 vel;

    bool _canMove = true;

    void Update()
    {
        if (_canMove)
        {
            transform.position = Vector3.SmoothDamp(transform.position, Player.Instance.transform.position, ref vel, _laserTime);
        }
    }

    private void Awake()
    {
        _visuals = GetComponentInChildren<laserVisuals>();
        _col = GetComponent<Collider>();
        _col.enabled = false;
        truc();
    }

    private void Start()
    {
        LifetimeMultiplier = Mathf.Lerp(1, 1f/3f, GameManager.Difficulty);
    }

    async void truc()
    {
        await Task.Delay((int)(1000f * LifetimeMultiplier));
        _canMove = false;
        SFXManager.Instance.PlaySFXClip(_laserBeepSound, transform.position, _laserBeepVolume);
        await _visuals.startBlinking(1f* LifetimeMultiplier);
        //await Task.Delay(4000);
        _col.enabled = true;
        SFXManager.Instance.PlaySFXClip(_laserShootSound1, transform.position, _laserShootVolume1);
        SFXManager.Instance.PlaySFXClip(_laserShootSound2, transform.position, _laserShootVolume2);
        await _visuals.shoot(.2f);
        Destroy(gameObject);
    }

}
