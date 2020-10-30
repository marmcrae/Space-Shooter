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

    private float _canFire = -1;
    private Player _player;
    private Animator _animator;

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
            Debug.LogError("Ainmator is NULL");
        }
    }


    // Update is called once per frame
    void Update()
    {
        EnemyBehavior();    
        
        if (Time.time > _canFire)
        {
            _fireRate = (Random.Range(3f, 7f));
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
       
        transform.Translate(Vector3.down * _enemySpeed * Time.deltaTime);
        
        float xRandom = Random.Range(-8, 8);
        if (transform.position.y < -5.5f)
        {
            transform.position = new Vector3(xRandom, 7f, 0);
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {

        Debug.Log("the object is " + other.name + "tag is " + other.tag);

        if(other.tag == "Player")
        {
            if (_player != null)
            {
                _player.Damage();
            }

            Destroy(this.gameObject, 2.3f);
            Debug.Log("Hit by player: Enemy.cs line 84");
            _enemySpeed = 0.5f;
            _animator.SetTrigger("OnEnemyDeath");
        }



        if(other.tag == "Laser")
        { 
            Destroy(other.gameObject);

            if (_player != null ) 
            {
                Debug.Log("Enemy.cs line 96");
                Destroy(this.gameObject, 2.3f);
                _player.AddPoints(10);
                Debug.Log("Hit by Laser. Enemy.cs line 102");
                _enemySpeed = 0.7f;
                _animator.SetTrigger("OnEnemyDeath");
            }

            Destroy(GetComponent<Collider2D>());
        }

       
       

        
    }
}
