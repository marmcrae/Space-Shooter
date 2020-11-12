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
    private float _spawnWaitTime = 5f;

    [SerializeField]
    private GameObject[] _enemies;

    private int _maxEnemies = 1;
    private int _enemyInstantiationCount = 0;
    private int _enemyCount = 0;
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
    

                int randomNum = Random.Range(1, 4);
                if (randomNum == 1)
                {
                 _enemy.GetComponent<Enemy>().EnemyShield();

                }else if(randomNum == 2)
                {
                    _enemyInstantiationCount += 1;
                    _enemy = Instantiate(_enemies[2], new Vector3(Random.Range(-8f, 8f), 7, 0), Quaternion.identity);
                }
                else if (randomNum == 3)
                {
                    _enemy.GetComponent<Enemy>()._enemyAvoidShot = true;
                }

                yield return new WaitForSeconds(_spawnWaitTime);
            }
      
            _stopSpawning = true;
            _enemyInstantiationCount = 0;

            if (_enemyCount <= 0)
            {
                //_enemyCount = 0;
                _uiManager.UpdateLevel(_waveNumber);
                _waveNumber += 1;
                _maxEnemies += 1;
                _stopSpawning = false;

                yield return new WaitForSeconds(2f);
            }

            yield return null;
        }

        if(_waveNumber == 5)
        {
            BossWave();
        }
    }

    IEnumerator SpawnPowerUpRoutine()
    {
        yield return new WaitForSeconds(2f);
        Debug.Log("PowerUp called");

        while (_stopSpawning == false)
        {
            int randomPowerUp = Random.Range(0, 11);
            Debug.Log("random number: " + randomPowerUp);

            if (randomPowerUp <= 2)
            //6 = super laser | 7 = missile
            {
                Instantiate(_powerUp[Random.Range(6, 8)], new Vector3(Random.Range(-8f, 8f), 7, 0), Quaternion.identity);
                yield return new WaitForSeconds(3f);
                Debug.Log("random number 6 /  7: ");


            }
            else if (randomPowerUp > 2 && randomPowerUp < 7)
            //5 = Ammo Boost | 4 = health
            {
                Instantiate(_powerUp[Random.Range(4, 6)], new Vector3(Random.Range(-8f, 8f), 7, 0), Quaternion.identity);
                yield return new WaitForSeconds(1f);
                Debug.Log("random number 4 / 5 : ");
            }
            else
            {
                Instantiate(_powerUp[Random.Range(0, 4)], new Vector3(Random.Range(-8f, 8f), 7, 0), Quaternion.identity);
                yield return new WaitForSeconds(2f);
                Debug.Log("random number: 0 - 3");

            }
            yield return null;
        }
    }



    public void AddEnemyCount()
    {
        _enemyCount += 1;
    }

    public void DecEnemyCount()
    {
        _enemyCount -= 1;
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
