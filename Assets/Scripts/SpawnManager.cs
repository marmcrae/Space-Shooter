using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;

    [SerializeField]
    private GameObject _enemyZigZagPrefab;

    [SerializeField]
    private GameObject _enemyContainer;

    [SerializeField]
    private GameObject[] _powerUp;

    [SerializeField]
    private float _spawnWaitTime = 45f;

    [SerializeField]
    private GameObject[] _enemies;

    private int _maxEnemies;
    private int _enemyCount;
    private int _waveNumber;

    private float _waveCoolDown;
    private bool _stopSpawning = false;

    
    // Start is called before the first frame update
    void Start()
    {
   
    }

    IEnumerator SpawnEnemyWaves()
    {
        while (_waveNumber < 5) // 5th = boss wave
        {
            yield return new WaitForSeconds(_waveCoolDown);
            _stopSpawning = false;
            StartSpawning();
            while (_stopSpawning == false && (_enemyCount <= _maxEnemies))
            {
                Instantiate(_enemies[Random.Range(0, 2)], new Vector3(Random.Range(-8f, 8f), 7, 0), Quaternion.identity);
                _enemyCount += 1;
                yield return new WaitForSeconds(_spawnWaitTime);
            }

            _enemyCount = 0;
            _maxEnemies += 10;
            _waveNumber += 1;

            _stopSpawning = true;

            yield return null;
        }
    }

    public void ActivateSpawn()
    {
        StartCoroutine(SpawnEnemyWaves());
    }
    public void StartSpawning()
    {
       // StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerUpRoutine());
    }


    //IEnumerator SpawnEnemyRoutine()
    //{
        
    //        yield return new WaitForSeconds(1f);

    //        while (_stopSpawning == false && (_enemyCount <= _maxEnemies))
    //        {
       
    //            GameObject newEnemy = Instantiate(_enemyPrefab, new Vector3(Random.Range(-8f, 8f), 7, 0), Quaternion.identity);
    //            GameObject newEnemyZigZag = Instantiate(_enemyZigZagPrefab, new Vector3(Random.Range(-8f, 8f), 7, 0), Quaternion.identity, _enemyContainer.transform);
    //            newEnemy.transform.parent = _enemyContainer.transform;
    //            newEnemyZigZag.transform.parent = _enemyContainer.transform;
    //            _enemyCount++;
    //            yield return new WaitForSeconds(_spawnWaitTime);
    //        }
    //}


    IEnumerator SpawnPowerUpRoutine()
    {
        yield return new WaitForSeconds(1f);

        while (_stopSpawning == false)
        {
            int randomPowerUp = Random.Range(0, 7);
            
            if( randomPowerUp == 6)
            //6 = super laser
            {
                Instantiate(_powerUp[randomPowerUp], new Vector3(Random.Range(-8f, 8f), 7, 0), Quaternion.identity);
                yield return new WaitForSeconds(15f);
            }
            else if(randomPowerUp == 5 || randomPowerUp == 4)
            //5 = negative boost | 4 = health boost
            {
                Instantiate(_powerUp[randomPowerUp], new Vector3(Random.Range(-8f, 8f), 7, 0), Quaternion.identity);
                yield return new WaitForSeconds(10f);
            }
            else if (randomPowerUp == 3)
            //3 = Ammo Boost
            {
                Instantiate(_powerUp[randomPowerUp], new Vector3(Random.Range(-8f, 8f), 7, 0), Quaternion.identity);
                yield return new WaitForSeconds(3f);
            }
            else
            {
                Instantiate(_powerUp[randomPowerUp], new Vector3(Random.Range(-8f, 8f), 7, 0), Quaternion.identity);
                yield return new WaitForSeconds(Random.Range(1.0f, 8.0f));
            }         
        } 
    }


    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}

/*
 
 IEnumerator SpawnEnemyWaves()
{


}
 
 
 
 
 
 
 
 
 
 
 */
 