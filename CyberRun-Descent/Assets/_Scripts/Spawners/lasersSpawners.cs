using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class lasersSpawners : MonoBehaviour
{
    public bool ShootLasers { get; set; }

    [SerializeField] float maxOffset; // disatnce max pour le spawn du laser en dessous et au dessus du joueur
    [SerializeField] float minWait; // temps d'attente mini entre 2 lasers
    [SerializeField] float maxWait; // temps d'attente max entre 2 lasers

    //[SerializeField] GameObject preLaser;
    [SerializeField] GameObject Laser;

    float yUp;
    float yDown;

    void Start()
    {
        StartShootingLasers();
    }

    private void Update()
    {
        yUp = Player.Instance.transform.position.y + maxOffset;
        yDown = Player.Instance.transform .position.y - maxOffset;
    }

    public void StartShootingLasers()
    {
        ShootLasers = true;
        StartCoroutine(SpawnLaser());
    }

    IEnumerator SpawnLaser()
    {
       while (ShootLasers)
        {
            yield return new WaitForSeconds(Random.Range(minWait, maxWait));
            Vector3 spawnPos = new Vector3(0, Random.Range(9, 1), 0);
            Instantiate(Laser, spawnPos,Quaternion.identity);
        }
    }

}
