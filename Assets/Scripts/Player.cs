﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.5f;

    [SerializeField]
    private float _lowThrusterSpeed = 1.5f;

    [SerializeField]
    private float _speedBoost = 2f;
   
    [SerializeField]
    private float _fireRate = 1f;

    [SerializeField]
    private int _lives = 3;

    [SerializeField]
    private int _shieldsRemaining = 3;

    [SerializeField]
    private int _score = 0;

    [SerializeField]
    public int _ammo = 100;

    [SerializeField]
    public int _maxAmmo = 100;

    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _superLaserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private GameObject _homingMissilePrefab;

    [SerializeField]
    private GameObject _shieldSprite;
    [SerializeField]
    private GameObject _ammoBoostSprite;
    [SerializeField]
    private GameObject _healthBoostSprite;
    [SerializeField]
    private GameObject _negativeSprite;
    [SerializeField]
    private GameObject _superLaserSprite;

    [SerializeField]
    private GameObject _leftEngineDown;
    [SerializeField]
    private GameObject _rightEngineDown;

    [SerializeField]
    private AudioClip _laserClip;
    private AudioSource _audioSource;


    private bool _isTripleShotActive = false;
    private bool _isSpeedPowerupActive = false;
    private bool _isAmmoBoostActive = false;
    private bool _isHealthBoostActive = false;
    private bool _isNegativeBoostActive = false;
    private bool _isSuperLaserActive = false;
    private bool _thrusterLow = false;
    private bool _isHomingMissileActive = false;
    public bool isShieldsActive = false;


    private float _canFire = 0f;
    private SpawnManager _spawnManager;
    private UIManager _uiManager;
    private ShakeBehavior _shakeBehavior;

    public float playerThrust = 100f;
    public float playerHealth = 100f;




    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _shakeBehavior = GameObject.Find("Main Camera").GetComponent<ShakeBehavior>();
        _audioSource = GetComponent<AudioSource>();

        if (_spawnManager == null)
        {
            Debug.LogError("Spawn Manager is NULL");
        }
        if (_uiManager == null)
        {
            Debug.LogError("The UI Manager is NULL");
        }
        if(_shakeBehavior == null)
        {
            Debug.LogError("Main Camera Shake behavior is NULL");
        }
        if( _audioSource == null)
        {
            Debug.LogError("The Audio Source Manager is Null");
        }
        else
        {
            _audioSource.clip = _laserClip;
        }           
    }


    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
        PowerUpCollect();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire && _isHomingMissileActive == false)
        {
            LaserBehavior();
        }
        
        if (Input.GetKeyDown(KeyCode.Space) && _isHomingMissileActive == true)
        {
            HomingMissile();
        }
    }

   
    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
        //ThrusterBehavior();

        if (!_isSpeedPowerupActive && _thrusterLow == false)
        {
            transform.Translate(direction * _speed * Time.deltaTime);
            ThrusterBehavior();

        }
        else if (!_isSpeedPowerupActive && _thrusterLow == true)
        {
            transform.Translate(direction * _lowThrusterSpeed * Time.deltaTime);
            ThrusterBehavior();
        }
        else
        {
            transform.Translate(direction * (_speed *  _speedBoost ) * Time.deltaTime);

        }
        
        
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), 0);

        if (transform.position.x > 11f)
        {
            transform.position = new Vector3(-11f, transform.position.y, 0);
        }
        else if(transform.position.x < -11f)
        {
            transform.position = new Vector3(11f, transform.position.y, 0);
        }
    }

    void ThrusterBehavior()
    {
        playerThrust -= .05f;

        if (playerThrust < 25)
        {
            _thrusterLow = true;
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            playerThrust = 100f;
            _thrusterLow = false;
        }
    }



    void LaserBehavior()
    {
        _canFire = Time.time + _fireRate;

        if (_ammo < 0)
        {
            _ammo = 0;
        }

        _uiManager.UpdateAmmo(_ammo);

        if (_ammo != 0 && _isNegativeBoostActive == false) 
        {
            _ammo--;

            if (_isTripleShotActive == true)
            {
                Instantiate(_tripleShotPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
            }
            else if(_isSuperLaserActive == true)
            {
                Instantiate(_superLaserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
            }
            else
            {
                Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
            }
            _audioSource.Play();
        }
    }

    public void Damage()
    {
        if(isShieldsActive == true)
        { 
            ShieldsBehavior();   
            return;
        }

        _shakeBehavior.TriggerShake();
        playerHealth -= 10;
        
        if(playerHealth == 0)
        {
            _lives -= 1;
            _uiManager.UpdateLives(_lives);
            playerHealth = 100f;
        }
      
        if(_lives < 0)
        {
            _lives = 0;
        }

        if(_lives == 2)
        {
            _rightEngineDown.SetActive(true);
        }
        else if(_lives == 1)
        {
            _leftEngineDown.SetActive(true);
        }

         if (_lives == 0)
        {
            _spawnManager.OnPlayerDeath();
            Debug.Log("Player Death" );
            Destroy(this.gameObject);
        }
    }

    public void ShieldsBehavior()
    {
        _shieldsRemaining -= 1;

        if (_shieldsRemaining > 0)
        {
            _uiManager.UpdateShield(_shieldsRemaining);
        }
        else if (_shieldsRemaining <= 0)
        {
            _shieldsRemaining = 0;
            isShieldsActive = false;
            _shieldSprite.gameObject.SetActive(false);
            _uiManager.UpdateShield(_shieldsRemaining);
        }
    }

    void PowerUpCollect()
    {
        if (Input.GetKey(KeyCode.C) && (GameObject.FindWithTag("PowerUp") != null))
        {
            GameObject.FindWithTag("PowerUp").transform.position = 
            Vector3.MoveTowards(GameObject.FindWithTag("PowerUp").transform.position, 
            this.gameObject.transform.position, 10f * Time.deltaTime);
        }
    }
    
    public void TripleShotActive()
    { 
          _isTripleShotActive = true;
          StartCoroutine(PowerDown());
    }
    public void SuperLaserActive()
    {
        _isSuperLaserActive = true;
        StartCoroutine(PowerDown());
    }
    IEnumerator PowerDown()
    {
        yield return new WaitForSeconds(5f);
        _isTripleShotActive = false;
        _isSuperLaserActive = false;
    }

 
    public void SpeedBoostActive()
    {
        _isSpeedPowerupActive = true;
        _speed *= _speedBoost;
        StartCoroutine(SpeedBoostPowerDown());
    }
    IEnumerator SpeedBoostPowerDown()
    {
        yield return new WaitForSeconds(5.0f);
        _isSpeedPowerupActive = false;
        _speed /= _speedBoost;
    }


    public void ShieldBoostActive()
    {
        isShieldsActive = true;
        _shieldSprite.gameObject.SetActive(true);
        _shieldsRemaining = 3;
        _uiManager.UpdateShield(_shieldsRemaining);
    }


    public void AmmoBoost()
    {
        _isAmmoBoostActive = true;
        _ammo = 100;
        _uiManager.UpdateAmmo(_ammo);
    }

    public void HealthBoost()
    {
        _isHealthBoostActive = true;

        if(_lives == 2) 
        {
            playerHealth = 100f;
            _lives += 1;
            _uiManager.UpdateLives(_lives);
            _rightEngineDown.SetActive(false);
            _leftEngineDown.SetActive(false);
        }
        else if(_lives == 1)
        {
            playerHealth = 100f;
            _lives += 1;
            _uiManager.UpdateLives(_lives);
            _rightEngineDown.SetActive(false);
        }
        
        if (_lives >= 3)
        {
            playerHealth = 100f;
            _lives = 3;
        }
    }

    public void NegativeBoost()
    {
        _isNegativeBoostActive = true;
        StartCoroutine(SetNegativeToFalse());

        IEnumerator SetNegativeToFalse()
        {
            yield return new WaitForSeconds(5f);
            _isNegativeBoostActive = false;
        }
    }

    public void HomingMissile()
    {
        Instantiate(_homingMissilePrefab, new Vector3(Random.Range(-8f, 8f), -4, 0), Quaternion.identity);

        StartCoroutine(SetHomingToFalse());

        IEnumerator SetHomingToFalse()
        {
            yield return new WaitForSeconds(5f);
            _isHomingMissileActive = false;
        }
    }

    public void AddPoints(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }

    public void ThrusterBoost()
    {
        if(Input.GetKey(KeyCode.LeftShift))
        {
            _speed *= 1.4f;
        }
    }
}


