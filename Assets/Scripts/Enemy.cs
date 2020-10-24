using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _enemySpeed = 4f;
   

    // Update is called once per frame
    void Update()
    {
        EnemyBehavior();          
    }

    void EnemyBehavior()
    {
        //set speed
        transform.Translate(Vector3.down * _enemySpeed * Time.deltaTime);


        //wrap & random
        float xRandom = Random.Range(-8, 8);

        if (transform.position.y < -5.5f)
        {
            transform.position = new Vector3(xRandom, 7f, 0);
        }
    }


    private void OnTriggerEnter(Collider other)
    {

        Player player = other.transform.GetComponent<Player>();

        if(player != null)
        {
            player.Damage();
        }


        if(other.tag == "Player")
        {
            Destroy(this.gameObject);
        }

        if(other.tag == "Laser")
        {
            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }
    }
}
