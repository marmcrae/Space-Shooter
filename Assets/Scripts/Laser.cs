using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{

    [SerializeField]
    private float _laserSpeed = 8f;

    [SerializeField]
    public bool _isEnemyLaser = false;

    // Update is called once per frame
    void Update()
    {

        if(_isEnemyLaser == false)
        {
           ShootUp();
        }
        else
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
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && _isEnemyLaser == true)
        {
            Player player = other.GetComponent<Player>();

            if ( player != null)
            {
                player.Damage();
            }
        }
    }

}
