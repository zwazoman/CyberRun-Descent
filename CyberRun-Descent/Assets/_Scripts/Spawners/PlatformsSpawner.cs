using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        while (IsSpawning && Platforms.Length>0)
        {
            Instantiate(Platforms[Random.Range(0, Platforms.Length)], _spawnSocket.position, Quaternion.identity);
            yield return new WaitForSeconds(Mathf.Lerp(_minTime, _maxTime,Mathf.Pow(Random.value,GameManager.Difficulty)));
        }
    }


}
