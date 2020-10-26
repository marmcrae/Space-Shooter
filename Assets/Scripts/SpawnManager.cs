﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;

    [SerializeField]
    private float spawnWaitTime = 5.0f;

    [SerializeField]
    private GameObject _enemyContainer;


    private bool _stopSpawning = false;
   


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    // Update is called once per frame
    void Update()
    {
         
    }

    IEnumerator SpawnRoutine()
    {
        while (_stopSpawning == false)
        {
            yield return new WaitForSeconds(spawnWaitTime);
            GameObject newEnemy = Instantiate(_enemyPrefab,  new Vector3(Random.Range(-8f, 8f), 7, 0), Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
        }
      
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}