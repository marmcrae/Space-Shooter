using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.5f;
 
    [SerializeField]
    private float _speedBoost = 2f;
   
    [SerializeField]
    private float _fireRate = 1f;

    [SerializeField]
    private int _lives = 3;

    [SerializeField]
    private int _score = 0;

    [SerializeField]
    private GameObject _laserPrefab;
   
    [SerializeField]
    private GameObject _tripleShotPrefab;

    [SerializeField]
    private GameObject _shieldSprite;


    private bool _isTripleShotActive = false;
    private bool _isSpeedPowerupActive = false;
    private bool _isShieldsActive = false;

    private float _canFire = 0f;
    private SpawnManager _spawnManager;
    private UIManager _uiManager;



    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();


        if (_spawnManager == null)
        {
            Debug.LogError("Spawn Manager is NULL");
        }
        if (_uiManager == null)
        {
            Debug.LogError("The UI Manager is NULL");
        }
    }


    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            LaserBehavior();
        }    
    }


    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        if(!_isSpeedPowerupActive)
        {
            transform.Translate(direction * _speed * Time.deltaTime);
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


    void LaserBehavior()
    {
        _canFire = Time.time + _fireRate;

        if (_isTripleShotActive == true)
        {
            Instantiate(_tripleShotPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
        }  
    }
 

    public void Damage()
    {
        if(_isShieldsActive == true)
        {   
            _isShieldsActive = false;
            _shieldSprite.gameObject.SetActive(false);
            return;
        }
   
        _lives -= 1;
        _uiManager.UpdateLives(_lives);

        if(_lives < 0)
        {
            _lives = 0;
        }

         if (_lives == 0)
        {
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
        }
    }


    public void TripleShotActive()
    { 
           _isTripleShotActive = true;
            StartCoroutine(PowerDown());
    }
    IEnumerator PowerDown()
    {
        yield return new WaitForSeconds(5f);
        _isTripleShotActive = false;
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
        _isShieldsActive = true;
        _shieldSprite.gameObject.SetActive(true);
    }

    public void AddPoints(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }
}


