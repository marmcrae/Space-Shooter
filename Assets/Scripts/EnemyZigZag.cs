using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyZigZag : MonoBehaviour
{
    Vector3 position = new Vector3();
    float t = 0f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        position.x = 2f * Mathf.Cos(t);
        position.y += 5f * Time.deltaTime;

        t += Time.deltaTime;
    }
}
