﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriplePowerUp : MonoBehaviour
{

    [SerializeField]
    private float _powerUpSpeed = 3f;

    [SerializeField]// 0 = Triple shot; 1 = Speed; 2 = Shields
    private int powerupId;



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
          
            switch (powerupId)
            {
                case 0:
                    player.TripleShotActive();
                    break;

                case 1:
                    player.SpeedBoostActive();
                    break;

                case 2:
                    player.ShieldBoostActive();
                    break;

                default:
                    Debug.Log("Invalid Id");
                    break;
            }
          
            Destroy(this.gameObject);
        }       
    }
}
