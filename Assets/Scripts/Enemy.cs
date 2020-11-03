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
    private float _frequency = 1.0f;
   
    [SerializeField]
    private float _magnitude = 15.0f;
    
    [SerializeField]
    private float _speed = 5.0f;

    private float _canFire = -1;
    private Player _player;
    private Animator _animator;

    Vector3 position = new Vector3();
    Vector3 pos;
    Vector3 axis;


    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        
        if(_player == null)
        {
            Debug.LogError("Player is NULL");
        }

        _animator = GetComponent<Animator>();

        if (_animator == null)
        {
            Debug.LogError("Animator is NULL");
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


    void EnemyShield()
    {
        //need to randomly generate shield when instantiating
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
                Debug.Log(" Green Enemy Behavior position.y: " + transform.position.y);
            }
        }  
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            if (_player != null)
            {
                _player.Damage();
            }

            Destroy(this.gameObject, 2.3f);
            _enemySpeed = 0f;
            _frequency = 0f;
            _animator.SetTrigger("OnEnemyDeath");
        }

        if (other.tag == "PowerUp")
        {
            Destroy(other.gameObject);  
        }

        if (other.tag == "Laser")
        { 
            Destroy(other.gameObject);

            if (_player != null ) 
            {
                Destroy(this.gameObject, 2.3f);
                _player.AddPoints(10);
                _enemySpeed = 0f;
                _frequency = 0f;
                _animator.SetTrigger("OnEnemyDeath");
            }

            Destroy(GetComponent<Collider2D>());
        }           
    }
}
