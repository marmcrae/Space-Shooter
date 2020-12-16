using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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
    [SerializeField]
    private GameObject _flashPrefab;

    private bool _isFlashActive = false;


    private Transform _enemyContainer;
    private Animator _animator;
    


    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _enemyContainer = GameObject.Find("Enemy Container").transform;
        _rigidbody = GetComponent<Rigidbody2D>();

        foreach(Transform enemy in _enemyContainer)
        { 
            if (enemy.GetComponent<Enemy>()._isExploded == false)
            {
                target = enemy;
                return;
            }
        }

        if (_animator == null)
        {
            Debug.LogError("Animator is NULL");
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(target != null)
        {
            Vector2 direction = (Vector2)target.position - _rigidbody.position;
            direction.Normalize();
            float rotateAmount = Vector3.Cross(direction, transform.up).z;
            _rigidbody.angularVelocity = -rotateAmount * _rotateSpeed;
            Vector3.Cross(direction, transform.up);
            _rigidbody.velocity = transform.up * _speed;
        } 
    }

    public void OnMissileDestroy()
    {
        Debug.Log("OnMissile destry called");
        _animator.SetTrigger("OnDestroy");
        _isFlashActive = true;

        if(_isFlashActive == true)
        {
            StartCoroutine(StartFlash());
        }
    }

    IEnumerator StartFlash()
    {
        while (true)
        {
   
            _flashPrefab.gameObject.SetActive(true);
            yield return new WaitForSeconds(.25f);
            _flashPrefab.gameObject.SetActive(false);
            yield return new WaitForSeconds(.25f);
            _flashPrefab.gameObject.SetActive(true);
            yield return new WaitForSeconds(.25f);
            _flashPrefab.gameObject.SetActive(false);
            _isFlashActive = false;
        }  
    }
}

 
