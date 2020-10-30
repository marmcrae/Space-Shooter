using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PumpkinEnemy : MonoBehaviour
{
    [SerializeField]
    private float _rotationSpeed = 30f;

    [SerializeField]
    private float _pumpkinSpeed = 3f;

    [SerializeField]
    private GameObject _explosionPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * _rotationSpeed * Time.deltaTime);
        transform.Translate(Vector3.down * _pumpkinSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Laser")
        {
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }
    }
}
