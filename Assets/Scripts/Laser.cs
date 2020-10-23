using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{

    [SerializeField]
    private float _laserspeed = 8f;


    // Update is called once per frame
    void Update()
        {
            LaserShoot();
            LaserDestroy();
        }

    void LaserShoot()
        {
            transform.Translate(Vector3.up * _laserspeed * Time.deltaTime);
        }

    void LaserDestroy()
    {
        if (transform.position.y > 8f)
        {
            Destroy(this.gameObject);
        }
    }

}
