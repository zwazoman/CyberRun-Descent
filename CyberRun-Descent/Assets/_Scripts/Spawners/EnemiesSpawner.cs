using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesSpawner : MonoBehaviour
{
    [Header("parameters")]
    [SerializeField] float _minTime;
    [SerializeField] float _maxTime;
    [SerializeField] float _spawnX;

    [Header("references")]
    [SerializeField] GameObject _enemy;
    [SerializeField] Transform _xSocket;
    [SerializeField] Transform _upSocket;
    [SerializeField] Transform _downSocket;

    [Header("EnnemyParam")]
    [SerializeField] float _enemySmoothTime;
    [SerializeField] public int _enemyShotsNumber;
    [SerializeField] public float _enemyTimeBetweenShots;
    [SerializeField] public float _enemyMoveSpeed;

    float _enemyMaxX;
    float _spawnMaxY;
    float _spawnMinY;

    public bool IsSpawning { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        StartSpawnEnemies();
        _enemyMaxX = _xSocket.position.x;
        _spawnMaxY = _upSocket.position.y;
        _spawnMinY = _downSocket.position.y;
    }

    public void StartSpawnEnemies()
    {
        IsSpawning = true;
        StartCoroutine(SpawnEnemy());
    }


    IEnumerator SpawnEnemy()
    {
        while (IsSpawning)
        {
            yield return new WaitForSeconds(Random.Range(_minTime, _maxTime));
            Vector3 spawnPos = new Vector3 (_spawnX, Random.Range(_spawnMinY, _spawnMaxY), 0);
            GameObject enemy = Instantiate(_enemy, spawnPos, Quaternion.identity);
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            enemyScript.MaxX = _enemyMaxX;
            enemyScript.SmoothTime = _enemySmoothTime;
            enemyScript.ShotsNumber = _enemyShotsNumber;
            enemyScript.TimeBetweenShots = _enemyTimeBetweenShots;
            enemyScript.MoveSpeed = _enemyMoveSpeed;
        }
    }

}
