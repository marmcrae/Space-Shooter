using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private GameObject _laserPrefab;
   
    [SerializeField]
    private GameObject _tripleShotPrefab;

    [SerializeField]
    private bool _isTripleShotActive = false;
    
    [SerializeField]
    private bool _isSpeedPowerupActive = false;

    
    private float _canFire = 0f;
    private SpawnManager _spawnManager;



    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();

        if (_spawnManager == null)
        {
            Debug.LogError("Spawn Manager is NULL");
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
        else if (transform.position.x < -11f)
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
        _lives -= 1;

        if (_lives <= 0)
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
 

}


