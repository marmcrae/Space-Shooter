using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriplePowerUp : MonoBehaviour
{

    [SerializeField]
    private float _powerUpSpeed = 3f;


    // Update is called once per frame
    void Update()
    {
        PowerUpBehavior();
    }

    void PowerUpBehavior()
    {
        transform.Translate(Vector3.down * _powerUpSpeed * Time.deltaTime);

        if(transform.position.y < -5.5)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Player player = other.transform.GetComponent<Player>();

        if(player != null)
        {
            player.TripleShotActive();
            Destroy(this.gameObject);
        }       
    }
}
