using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _enemySpeed = 4f;

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
        
        if(other.tag == "Player")
        {
            if (_player != null)
            {
                _player.Damage();
            }

            Destroy(this.gameObject, 2.3f);
            _enemySpeed = 0.7f;
            _animator.SetTrigger("OnEnemyDeath");
        }


        if(other.tag == "Laser")
        {
            Destroy(other.gameObject);

            if (_player != null) 
            {
                _player.AddPoints(10);
            }

            Destroy(this.gameObject, 2.3f);
            _enemySpeed = 0.7f;
            _animator.SetTrigger("OnEnemyDeath");
        }
    }
}
