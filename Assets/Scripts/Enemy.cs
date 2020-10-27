using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _enemySpeed = 4f;

    private Player _player;

    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
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
                Destroy(this.gameObject);
                Debug.Log("Object destroyed");
            }
        }


        if(other.tag == "Laser")
        {
            if (_player != null) 
            {
                Destroy(other.gameObject);
                _player.AddPoints(Random.Range(5, 13));
                Destroy(this.gameObject);
            } 
        }
    }
}
