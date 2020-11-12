using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarWeapon : MonoBehaviour
{
    [SerializeField]
    private float _laserSpeed = 15f;

    [SerializeField]
    public bool _isEnemyLaser = false;

    [SerializeField]
    public bool _isEnemyLaserShootUp = false;

    public Player player;

    private void Start()
    {
        Player player = GameObject.Find("Player").GetComponent<Player>();

        if (player == null)
        {
            Debug.Log("Player is NULL");
        }
    }



    // Update is called once per frame
    void Update()
    {

        if (_isEnemyLaser == false || _isEnemyLaserShootUp == true)
        {
            ShootUp();
        }
        if (_isEnemyLaser == true && _isEnemyLaserShootUp == false)
        {
            ShootDown();
        }
    }

    void ShootUp()
    {

        transform.Translate(Vector3.up * _laserSpeed * Time.deltaTime);

        if (transform.position.y > 8f)
        {
            Destroy(this.gameObject);
        }
    }

    void ShootDown()
    {
        transform.Translate(Vector3.down * _laserSpeed * Time.deltaTime);

        if (transform.position.y < -8f)
        {
            Destroy(this.gameObject);
        }
    }



    public void EnemyLaserOn()
    {
        _isEnemyLaser = true;

        if (player != null)
        {
            if (player.transform.position.y > 0 && transform.position.y < 0)
            {
                _isEnemyLaserShootUp = true;
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && _isEnemyLaser == true)
        {
            Player player = other.GetComponent<Player>();

            if (player != null)
            {
                player.Damage();
            }
        }
        if (other.tag == "PowerUp" && _isEnemyLaser == true)
        {
            Destroy(other.gameObject);
        }
    }

}


