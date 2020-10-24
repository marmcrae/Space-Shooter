using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.5f;

    [SerializeField]
    private GameObject _laserPrefab;

    [SerializeField]
    private float _fireRate = 1f;

    [SerializeField]
    private int _lives = 3;

    private float _canFire = 0f;





    // Start is called before the first frame update
    void Start()
    {
        //take the current position and set it to (0, 0, 0)
        transform.position = new Vector3(0, 0, 0);
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

        //*******create user input*******
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // moves player one unit (meter) to the right, left, up, and down based off user input
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
        transform.Translate(direction * _speed * Time.deltaTime);


        //*******create player bounds & wrap*******

        //player bounds
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), 0);

        //player wrap
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
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1f, 0), Quaternion.identity);
        }

 

    public void Damage()
    {
        _lives -= 1;

        if (_lives <= 0)
        {
            Destroy(this.gameObject);
        }
    }

}


