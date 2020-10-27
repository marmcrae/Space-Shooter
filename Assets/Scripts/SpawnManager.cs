using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;

    [SerializeField]
    private GameObject _enemyContainer;

    [SerializeField]
    private GameObject[] _powerUp;

    [SerializeField]
    private float spawnWaitTime = 5.0f;

    
    private bool _stopSpawning = false;
    

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerUpRoutine());
    }


    IEnumerator SpawnEnemyRoutine()
    {
        while (_stopSpawning == false)
        {
            GameObject newEnemy = Instantiate(_enemyPrefab, new Vector3(Random.Range(-8f, 8f), 7, 0), Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(spawnWaitTime);
        }
      
    }


    IEnumerator SpawnPowerUpRoutine()
    {
        while (_stopSpawning == false)
        {
            int randomPowerUp = Random.Range(0, 3);
            Instantiate(_powerUp[randomPowerUp], new Vector3(Random.Range(-8f, 8f), 7, 0), Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3.0f, 8.0f));
        } 
    }


    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}
