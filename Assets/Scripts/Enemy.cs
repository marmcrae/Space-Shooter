﻿using System.Collections;
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
    private bool _isEnemyShieldActive = false;

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


    //void EnemyShield()
    //{
    //    //need to create in spawnManager random iteration of shields. 
    //    _isEnemyShieldActive = false;
    //    _enemyShieldSprite.gameObject.SetActive(false);

    //}


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
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player" )
        //{
        //    && _isEnemyShieldActive == true
        //    EnemyShield();
        //}
        //else
        {
            if (_player != null)
            {
                _player.Damage();
                Debug.Log("This is what is hitting you!!!! tag:" + this.gameObject);
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

        if (other.tag == "Player" )
        {
        //    EnemyShield();
        //&& _isEnemyShieldActive == true
        //}
        //else
        //{
        Destroy(other.gameObject);

            if (_player != null)
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
