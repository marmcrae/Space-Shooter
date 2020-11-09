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


    /**************AGGRESSIVE ENEMY*****************/

    public float aggEnemyMinRot = 80.0f;
    public float aggEnemyMaxRot = 120.0f;
    public float aggMinMoveSpeed = 1.75f;
    public float aggMaxMoveSpeed = 2.25f;
    [SerializeField]
    private float rotationSpeed = 75.0f; // Degrees per second
    [SerializeField]
    private float movementSpeed = 2.0f; // Units per second;
    private Transform target;
    private Quaternion quaternion;



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


        /**************Agg Enemy********************/
        target = GameObject.Find("Player").transform;
        rotationSpeed = Random.Range(aggEnemyMinRot, aggEnemyMaxRot);
        movementSpeed = Random.Range(aggMinMoveSpeed, aggMaxMoveSpeed);
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

            Vector3 v3 = target.position - transform.position;
            float angle = Mathf.Atan2(v3.y, v3.x) * Mathf.Rad2Deg;
            quaternion = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, quaternion, rotationSpeed * Time.deltaTime);
            transform.Translate(Vector3.right * movementSpeed * Time.deltaTime);
        }


        else if (tag == "BossEnemy")
        {
            transform.Translate(-1f, 1.4f, 0);
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
        yield return new WaitForSeconds(.25f);
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

       
            if (_bossLives == 0 && _player != null)
            {
                Destroy(this.gameObject, 1.5f);
                _enemySpeed = 0f;
                _frequency = 0f;
                _animator.SetTrigger("OnEnemyDeath");
                _uiManager.WinnerText();
            }

        }
    }
}
