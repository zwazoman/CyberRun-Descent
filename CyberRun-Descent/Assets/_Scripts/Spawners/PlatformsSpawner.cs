using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlatformsSpawner : MonoBehaviour
{
    public bool IsSpawning {get;set;}

    [SerializeField] float _minTime;
    [SerializeField] float _maxTime;

    [SerializeField] Transform _spawnSocket;
    [SerializeField] GameObject[] Platforms;
    void Start()
    {
       StartSpawnPlatforms();
    }

    public void StartSpawnPlatforms()
    {
        IsSpawning = true;
        StartCoroutine(SpawnPlatforms());
    }

    IEnumerator SpawnPlatforms()
    {
        while (IsSpawning)
        {
            yield return new WaitForSeconds(Random.Range(_minTime,_maxTime));
            Instantiate(Platforms[Random.Range(0, Platforms.Length)], _spawnSocket.position, Quaternion.identity);
        }
    }
}
