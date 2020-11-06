﻿using System.Collections;
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
    private float _spawnWaitTime = 10f;

    [SerializeField]
    private GameObject[] _enemies;

    private int _maxEnemies = 4;
    private int _enemyInstantiationCount = 0;
    public int _enemyCount = 0;
    private int _waveNumber = 0;

    private float _waveCoolDown = 1f;
    private bool _stopSpawning = false;

    private UIManager _uiManager;

    
    // Start is called before the first frame update
    void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

        if(_uiManager == null)
        {
            Debug.Log("UI Manager is NULL");
        }
    }

    IEnumerator SpawnEnemyWaves()
    {
        while (_waveNumber < 5) // 5th = boss wave
        {
            yield return new WaitForSeconds(_waveCoolDown);
    
            while (_stopSpawning == false && (_enemyInstantiationCount <= _maxEnemies))
            { 
                    Instantiate(_enemies[Random.Range(0, 2)], new Vector3(Random.Range(-8f, 8f), 7, 0), Quaternion.identity);
                    _enemyInstantiationCount += 1;
                    _enemyCount += 1;
                    Debug.Log("In Loop  EnemyInstantiation: " + _enemyInstantiationCount + " Enemy Count: " + _enemyCount);
                    yield return new WaitForSeconds(_spawnWaitTime);             
            }

            _enemyInstantiationCount = 0;
            
            Debug.Log(" Out of loop. Enemy count : " + _enemyCount + " Wave Count: " + _waveNumber);
         
            _maxEnemies += 5;

            if (_enemyCount == 0) 
            {
                _waveNumber += 1;
                _uiManager.UpdateLevel(_waveNumber);
            }
              
            Debug.Log("After Enemy Count == 0. Enemy count: " + _enemyCount +  "Max Enemy count : " + _maxEnemies + " Wave Count: " + _waveNumber);

            _stopSpawning = true;

           yield return null;
        }
    }

    public void ActivateSpawn()
    {
        StartCoroutine(SpawnEnemyWaves());
        StartCoroutine(SpawnPowerUpRoutine());
    }
 

   IEnumerator SpawnPowerUpRoutine()
    {
        yield return new WaitForSeconds(.5f);

        while (_stopSpawning == false)
        {
           int randomPowerUp = Random.Range(0, 7);
            Debug.Log("Spawn PwerUp");
            

        //    if (randomPowerUp == 6)
        //    //6 = super laser
        //    {
               //Instantiate(_powerUp[randomPowerUp], new Vector3(Random.Range(-8f, 8f), 7, 0), Quaternion.identity);
               
        //    }
        //    else if (randomPowerUp == 5)
        //    //5 = Ammo Boost
        //    {
        //        Instantiate(_powerUp[randomPowerUp], new Vector3(Random.Range(-8f, 8f), 7, 0), Quaternion.identity);
        //        // yield return new WaitForSeconds(4f);
        //    }
        //    else
        //    {
                Instantiate(_powerUp[randomPowerUp], new Vector3(Random.Range(-8f, 8f), 7, 0), Quaternion.identity);
                yield return new WaitForSeconds(Random.Range(1.0f, 8.0f));
         }
      }
   // }


    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
} 
