using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _enemySpeed = 4f;

    [SerializeField]
    private float _fireRate = 3.0f;

    [SerializeField]
    private GameObject _laserPrefab;

    [SerializeField]
    private GameObject _enemyShieldSprite;

    [SerializeField]
    private float _frequency = 1.0f;
   
    [SerializeField]
    private float _magnitude = 15.0f;
    
    [SerializeField]
    private float _speed = 5.0f;

    [SerializeField]
    private float _bossFrequency = 1.0f;

    [SerializeField]
    private float _bossMagnitude = 15.0f;

    [SerializeField]
    public float _bossSpeed = 5.0f;

    [SerializeField]
    private bool _isEnemyShieldActive = false;

    [SerializeField]
    private bool _bossTransportActive = false;

    [SerializeField]
    private bool _bossFlickerActive = false;

    private float _canFire = -1;
    private int _bossLives = 20;
        

    private Player _player;
    private Animator _animator;
    private SpawnManager _spawnManager;
    private ShakeBehavior _shakeBehavior;
    private UIManager _uiManager;

    Vector3 position = new Vector3();
    Vector3 pos;
    Vector3 axis;


    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _shakeBehavior = GameObject.Find("Main Camera").GetComponent<ShakeBehavior>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

        if (_uiManager == null)
        {
            Debug.Log("UI Manager is NULL");
        }

        if (_player == null)
        {
            Debug.LogError("Player is NULL");
        }

        _animator = GetComponent<Animator>();

        if (_animator == null)
        {
            Debug.LogError("Animator is NULL");
        }

        if (_spawnManager == null)
        {
            Debug.LogError("Spawn Manager is NULL");
        }
        if (_shakeBehavior == null)
        {
            Debug.LogError("Main Camera Shake behavior is NULL");
        }

        pos = transform.position;
        axis = transform.right;
    }


    // Update is called once per frame
    void Update()
    {
        EnemyBehavior();
        LaserFire();   
    }


    void LaserFire()
    {
        if (Time.time > _canFire)
        {
            _fireRate = (Random.Range(1f, 4f));
            _canFire = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);

            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].EnemyLaserOn();
            }
        }
    }


    void EnemyBehavior()
    { 
        if(tag == "OrgEnemy")
        {
            transform.Translate(Vector3.down * _enemySpeed * Time.deltaTime);

            float xRandom = Random.Range(-8, 8);

            if (transform.position.y < -5.5f)
            {
                transform.position = new Vector3(xRandom, 7f, 0);
            }          
        }  
        

        else if(tag == "GreenEnemy")
        {
            pos += Vector3.down * Time.deltaTime * _speed;
            transform.position = pos + axis * Mathf.Sin(Time.time * _frequency) * _magnitude;

            if (transform.position.y < -5.5f)
            {
                float xRandom = Random.Range(-8, 8);
                pos = new Vector3(xRandom, 7f, 0);
                transform.Translate(Vector3.down * Time.deltaTime * _speed);
                transform.position = pos  * Mathf.Sin(Time.time * _frequency) * _magnitude;
            }
        }


        else if (tag == "AggressiveEnemy")
        { 
            transform.Translate(Vector3.down * _enemySpeed * Time.deltaTime);
            var _aggressiveEnemyPos = this.gameObject.transform.position;
            float _strikingDistance = Vector3.Distance(_aggressiveEnemyPos, _player.transform.position);

            if (_strikingDistance <= 5f)
            {
              Vector3.MoveTowards(_player.transform.position, this.gameObject.transform.position, 10f * Time.deltaTime);
            }

            float xRandom = Random.Range(-8, 8);

            if (transform.position.y < -5.5f)
            {
                transform.position = new Vector3(xRandom, 7f, 0);
            }
        }


        else if (tag == "BossEnemy")
        {
            float randoX = Random.Range(-8, 8);
            float randoY = Random.Range(3, 5);
            position = new Vector3(randoX, randoY, 0);

            if (_bossTransportActive == false)
            {
                StartCoroutine(BossTransport());
                _bossTransportActive = true;
            }
        }
    }


    IEnumerator BossTransport()
    {
        yield return new WaitForSeconds(.20f);
        transform.position = Vector3.Lerp(position, position, 6);
        _bossTransportActive = false;
    }


    public void EnemyShield()
    {
        _isEnemyShieldActive = true;
        _enemyShieldSprite.gameObject.SetActive(true);
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player"  && tag != "BossEnemy")

        {
            if (_player != null)
            {
                _player.Damage();
            }

            Destroy(this.gameObject, 2.3f);
            _spawnManager.enemyCount--;
            _enemySpeed = 0f;
            _frequency = 0f;
            _animator.SetTrigger("OnEnemyDeath");
        }


        if (other.tag == "Laser" && tag != "BossEnemy" && _isEnemyShieldActive == false)
        {
      
         Destroy(other.gameObject);

            if (_player != null)
            {
                Destroy(this.gameObject, 1.5f);
                _spawnManager.enemyCount--;
                _player.AddPoints(10);
                _enemySpeed = 0f;
                _frequency = 0f;
                _animator.SetTrigger("OnEnemyDeath");
            }

            Destroy(GetComponent<Collider2D>());
        }


        if (other.tag == "Laser" || other.tag == "Player" && tag == "EnemyShield" && _isEnemyShieldActive == true)
        {
            Destroy(other.gameObject);
            _isEnemyShieldActive = false;
            _enemyShieldSprite.gameObject.SetActive(false);
            _player.AddPoints(10);
        }

        if (tag == "BossEnemy" && other.tag == "Player" || other.tag == "Laser")
        {
            _shakeBehavior.TriggerShake();
            _bossLives--;
            _player.AddPoints(10);

            //if (_bossFlickerActive == false)
            //{
            //    StartCoroutine(BossDamageFlicker());
            //    _bossTransportActive = true;
            //}

            if (_bossLives == 0 && _player != null)
            {
                _animator.SetTrigger("OnEnemyDeath");
                Destroy(this.gameObject, 1.5f);
                _enemySpeed = 0f;
                _frequency = 0f;
                _uiManager.WinnerText();
            }
        }
    }


    //IEnumerator BossDamageFlicker()
    //{
    //    while (true)
    //    {
    //        gameObject.SetActive(false);
    //        Debug.Log("bossFlicker game object false");
    //        yield return new WaitForSeconds(0.5f);
    //        gameObject.SetActive(true);
    //        Debug.Log("bossFlicker game object true");

    //        _bossFlickerActive = false;
    //    }

    //}
}
