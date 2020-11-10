using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class HomingMissile : MonoBehaviour
{
    public Transform target;
    private Rigidbody2D _rigidbody;
    [SerializeField]
    private float _speed = 5f;
    [SerializeField]
    private float _rotateSpeed = 200f;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("OrgEnemy").transform;
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 direction = (Vector2)target.position - _rigidbody.position;
        direction.Normalize();
        float rotateAmount = Vector3.Cross(direction, transform.up).z;
        _rigidbody.angularVelocity = -rotateAmount * _rotateSpeed;
        Vector3.Cross(direction, transform.up);
        _rigidbody.velocity = transform.up * _speed;
    }
}

 
