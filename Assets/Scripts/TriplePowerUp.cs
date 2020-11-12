using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriplePowerUp : MonoBehaviour
{

    [SerializeField]
    private float _powerUpSpeed = 3f;

    [SerializeField]
    // 0 = Triple shot; 1 = Speed; 2 = Shields; 3 = Ammo; 4 = Health; 5 = Negative;
    private int powerupId;

    [SerializeField]
    private AudioClip _powerClip;

    [SerializeField]
    private GameObject _homingMissilePrefab;

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

        AudioSource.PlayClipAtPoint(_powerClip, transform.position);

        if (other.tag == "EnemyLaser")
        {
            Destroy(this.gameObject);
        }

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

                case 3:
                    player.NegativeBoost();
                    break;

                case 4:
                    player.HealthBoost();
                    break;

                case 5:
                    player.AmmoBoost();
                    break;

                case 6:
                    player.SuperLaserActive();
                    break;

                case 7:
                    player.HomingMissile();
                    break;

                default:
                    Debug.Log("Invalid Id");
                    break;
            }
          
            Destroy(this.gameObject);
        }       
    }
}
