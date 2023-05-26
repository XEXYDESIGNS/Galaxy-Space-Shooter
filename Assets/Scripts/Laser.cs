﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private int _laserSpeed = 8;
    
    void Update()
    {
        transform.Translate(Vector3.up * _laserSpeed * Time.deltaTime);
        
        if (transform.position.y > 7f)
        {
            Destroy(gameObject);
        }
    }
}
