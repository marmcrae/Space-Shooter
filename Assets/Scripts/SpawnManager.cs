using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _bossEnemyPrefab;

    [SerializeField]
    private GameObject _enemyContainer;

    [SerializeField]
    private GameObject[] _powerUp;

    [SerializeField]
    private float _spawnWaitTime = 10f;

    [SerializeField]
    private GameObject[] _enemies;

    private int _maxEnemies = 1;
    private int _enemyInstantiationCount = 0;
    public int enemyCount = 0;
    public int bossEnemyCount = 0;
    private int _waveNumber = 1;

    private float _waveCoolDown = 1f;
    private bool _stopSpawning = false;

    private UIManager _uiManager;
    private GameObject _enemy;
    private GameObject _bossEnemy;

    
    // Start is called before the first frame update
    void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        
        if(_uiManager == null)
        {
            Debug.Log("UI Manager is NULL");
        }
    }

    private void Update()
    {
  

    }


    public void ActivateSpawn()
    {
        StartCoroutine(SpawnEnemyWaves());
        StartCoroutine(SpawnPowerUpRoutine());
    }


    IEnumerator SpawnEnemyWaves()
    {
        while (_waveNumber < 5) // 5th = boss wave
        {
            yield return new WaitForSeconds(_waveCoolDown);

            while (_stopSpawning == false && _enemyInstantiationCount <= _maxEnemies)
            {
                _enemy = Instantiate(_enemies[Random.Range(0, 2)], new Vector3(Random.Range(-8f, 8f), 7, 0), Quaternion.identity);
                _enemyInstantiationCount += 1;
                enemyCount += 1;

                int randomNum = Random.Range(1, 4);
                if (randomNum == 1)
                {
                    _enemy.GetComponent<Enemy>().EnemyShield();
                }else if(randomNum == 2)
                {
                    enemyCount += 1;
                    _enemy = Instantiate(_enemies[2], new Vector3(Random.Range(-8f, 8f), 7, 0), Quaternion.identity);
                }

                yield return new WaitForSeconds(_spawnWaitTime);
            }
            Debug.Log("Out of while loop. Enemy Inst: " + _enemyInstantiationCount + " Enemy Count: " + enemyCount + " Wave Count: " + _waveNumber);


            _stopSpawning = true;
            _enemyInstantiationCount = 0;

            if (enemyCount <= 0)
            {
                enemyCount = 0;
                _uiManager.UpdateLevel(_waveNumber);
                _waveNumber += 1;
                _maxEnemies += 4;
                _stopSpawning = false;
                Debug.Log("Enemy == 0. Enemy Inst: " + _enemyInstantiationCount + " Enemy Count: " + enemyCount + " Wave Count: " + _waveNumber);

                yield return new WaitForSeconds(3f);
            }

            
            Debug.Log("After Max Enemy upped. Enemy Inst: " + _enemyInstantiationCount + " Enemy Count: " + enemyCount + " Wave Count: " + _waveNumber + "Max Enemies: " + _maxEnemies);

            yield return null;
        }

        if(_waveNumber == 5)
        {
            BossWave();
        }
    }


    IEnumerator SpawnPowerUpRoutine()
    {
        yield return new WaitForSeconds(15f);

        while (_stopSpawning == false)
        {
            int randomPowerUp = Random.Range(0, 101);       

            if (randomPowerUp < 15 )
            //6 = super laser
            {
                Instantiate(_powerUp[6], new Vector3(Random.Range(-8f, 8f), 7, 0), Quaternion.identity);

            }
            else if (randomPowerUp < 70 && randomPowerUp > 15)
            //5 = Ammo Boost
            {
                Instantiate(_powerUp[5], new Vector3(Random.Range(-8f, 8f), 7, 0), Quaternion.identity);
            }
            else
            {
                Instantiate(_powerUp[Random.Range(0 , 5)], new Vector3(Random.Range(-8f, 8f), 7, 0), Quaternion.identity);
                yield return new WaitForSeconds(Random.Range(1.0f, 8.0f));
            }
            yield return null;
        }
    }


    void BossWave()
    {     
        _bossEnemy = Instantiate(_bossEnemyPrefab, new Vector3(-1f, 1.4f, 0), Quaternion.identity);
        StartCoroutine(BossWaveWait());
    }


    IEnumerator BossWaveWait()
    {
        yield return new WaitForSeconds(10f);     
    }


    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
} 
