using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{

    [SerializeField]
    private float _laserSpeed = 8f;

    // Update is called once per frame
    void Update()
        {
            LaserShoot();
            LaserDestroy();
        }

    void LaserShoot()
        {
            transform.Translate(Vector3.up * _laserSpeed * Time.deltaTime);
        }

    void LaserDestroy()
    {
        if (transform.parent != null)
        {
            Destroy(transform.parent.gameObject);
        }
        if (transform.position.y > 8f)
        {
            Destroy(this.gameObject);
        }
    }

}
