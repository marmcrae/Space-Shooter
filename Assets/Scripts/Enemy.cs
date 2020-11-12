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
    public bool _isExploded { get; private set; } = false;

    [SerializeField]

    private bool _bossFlickerActive = false;

    [SerializeField]
    private bool _bossWinnerActive = false;

    [SerializeField]
    public bool _enemyAvoidShot = false;

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

    private GameObject _laserTaget;


    /**************AGGRESSIVE ENEMY*****************/
    public float aggEnemyMinRot = 80.0f;
    public float aggEnemyMaxRot = 120.0f;
    public float aggMinMoveSpeed = 1.75f;
    public float aggMaxMoveSpeed = 2.25f;
    [SerializeField]
    private float rotationSpeed = 75.0f; // Degrees per second
    [SerializeField]
    private float movementSpeed = 10.0f; // Units per second;
    private Transform playerTarget;
    private Quaternion quaternion;


    private void OnEnable()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _spawnManager.AddEnemyCount();
    }

    private void OnDisable()
    {
        _spawnManager.DecEnemyCount();
    }

    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>(); 
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
        playerTarget = GameObject.Find("Player").transform;
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
            EnemyAvoid();
        }  
        

        if(tag == "GreenEnemy")
        {
            pos += Vector3.down * Time.deltaTime * _speed;
            transform.position = pos + axis * Mathf.Sin(Time.time * _frequency) * _magnitude;
            EnemyAvoid();

            if (transform.position.y < -5.5f)
            {
                float xRandom = Random.Range(-8, 8);
                pos = new Vector3(xRandom, 7f, 0);
                transform.Translate(Vector3.down * Time.deltaTime * _speed);
                transform.position = pos  * Mathf.Sin(Time.time * _frequency) * _magnitude;
            }
        }


       if (tag == "AggressiveEnemy")
        {
            if (playerTarget != null)
            {
                Vector3 v3 = playerTarget.position - transform.position;
                float distance = Vector3.Distance(playerTarget.position, transform.position);
                float angle = Mathf.Atan2(v3.y, v3.x) * Mathf.Rad2Deg;
                quaternion = Quaternion.AngleAxis(angle, Vector3.forward);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, quaternion, rotationSpeed * Time.deltaTime);
                transform.Translate(Vector3.right * movementSpeed * Time.deltaTime);


                if (distance < 4f)
                {
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, quaternion, rotationSpeed * Time.deltaTime);
                    transform.Translate(Vector3.right * 30f * Time.deltaTime);
                }

            }
        }

        if ((tag == "OrgEnemy" || tag == "AggressiveEnemy" || tag == "GreenEnemy") && transform.position.y < -15)
        {
            Destroy(this.gameObject);
          
        }


        if (tag == "BossEnemy")
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
        yield return new WaitForSeconds(.35f);
        transform.position = Vector3.Lerp(position, position, 6);
        _bossTransportActive = false;
    }


    public void EnemyShield()
    {
        _isEnemyShieldActive = true;
        _enemyShieldSprite.gameObject.SetActive(true);
    }

    public void EnemyAvoid()
    {

        _laserTaget = GameObject.FindWithTag("Laser");

        if (_laserTaget != null)
        {

            float laserDistance = Vector3.Distance(transform.position, _laserTaget.transform.position);

            if (_laserTaget != null && _enemyAvoidShot == true)
            {
                if (laserDistance < 3f)
                {
                    transform.position = Vector3.MoveTowards(transform.position, _laserTaget.transform.position, -1 * 15f * Time.deltaTime);
                }
            }
        }
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
            _enemySpeed = 0f;
            _frequency = 0f;
            _animator.SetTrigger("OnEnemyDeath");
            _isExploded = true;
        }


        if (other.tag == "Laser" && tag != "BossEnemy" && _isEnemyShieldActive == false)
        { 
         Destroy(other.gameObject);

            if (_player != null)
            {
                Destroy(this.gameObject, 1.5f);
                _player.AddPoints(10);
                _enemySpeed = 0f;
                _frequency = 0f;
                _animator.SetTrigger("OnEnemyDeath");
                _isExploded = true;
                other.GetComponent<Collider2D>().enabled = false;
            }
        }


        if (other.tag == "HomingMissile" && tag != "BossEnemy" && _isEnemyShieldActive == false)
        {
            Destroy(other.gameObject);
            _animator.SetTrigger("OnDestroy");
            if (_player != null)
            {
                _enemySpeed = 0f;
                _frequency = 0f;
                Destroy(this.gameObject, 1.5f);
                _player.AddPoints(10);
                _animator.SetTrigger("OnEnemyDeath");
                _isExploded = true;
                other.GetComponent<Collider2D>().enabled = false;
            }
        }



        if (other.tag == "Laser" || other.tag == "Player" && tag == "EnemyShield" && _isEnemyShieldActive == true)
        {
            Destroy(other.gameObject);
            _isEnemyShieldActive = false;
            _enemyShieldSprite.gameObject.SetActive(false);
            _player.AddPoints(10);
        }


        if( tag == "AggressiveEnemy"  && other.tag == "Player")
        {

            if (_player != null)
            {
                _player.Damage();
            }

            movementSpeed = 0;
            rotationSpeed = 0;
            Destroy(this.gameObject, 2.3f);
            _animator.SetTrigger("OnEnemyDeath");
            _isExploded = true;
        }



        if (tag == "BossEnemy" && other.tag == "Player" || other.tag == "Laser")
        {
            _shakeBehavior.TriggerShake();
            _bossLives--;
            _player.AddPoints(10);

       
            if (_bossLives == 0 && _player != null)
            {
                _bossSpeed = 0f;
                _bossFrequency = 0f;
                _shakeBehavior.TriggerShake();
                Destroy(this.gameObject, 2.5f);
                _animator.SetTrigger("OnEnemyDeath");
                _isExploded = true;
                _bossWinnerActive = true;

                if (_bossWinnerActive == true)
                {
                    Debug.Log("boss winner start coroutine called");
                    StartCoroutine(WinnerTextCoroutine());
                }
            }
        }
    }

    IEnumerator WinnerTextCoroutine()
    {
        yield return new WaitForSeconds(2f);
        Debug.Log("Winner text called");
        _uiManager.WinnerText();          
    }
}
