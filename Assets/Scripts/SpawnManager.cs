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
    private float[] _spawnWave;

    
    private bool _stopSpawning = false;

    

    // Start is called before the first frame update
    void Start()
    {
   
    }

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerUpRoutine());
    }


    IEnumerator SpawnEnemyRoutine()
    {
        
            yield return new WaitForSeconds(1f);

            while (_stopSpawning == false)
            {
                GameObject newEnemy = Instantiate(_enemyPrefab, new Vector3(Random.Range(-8f, 8f), 7, 0), Quaternion.identity);
                GameObject newEnemyZigZag = Instantiate(_enemyZigZagPrefab, new Vector3(Random.Range(-8f, 8f), 7, 0), Quaternion.identity);
                newEnemy.transform.parent = _enemyContainer.transform;
                newEnemyZigZag.transform.parent = _enemyContainer.transform;
                yield return new WaitForSeconds(_spawnWaitTime);
            }
    }


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
                yield return new WaitForSeconds(2f);
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
 