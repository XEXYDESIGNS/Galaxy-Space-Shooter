using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLasers : MonoBehaviour
{
    private float _laserSpeed = 8.0f;
    private void Update()
    {
        transform.Translate(Vector3.down * _laserSpeed * Time.deltaTime);
        
        if (transform.position.y < -4f)
        {
            if (transform.parent == true)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }
}
