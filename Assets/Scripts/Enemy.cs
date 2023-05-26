using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab;

    //move enemy down at 4mps
    [SerializeField] private float _enemyMovement = 4f;

    //[SerializeField] private float _enemySpawnTime = 0.5f;

    [SerializeField] private float _nextSpawn = 0.5f;
    //when it goes off the screen, respawn at the top

    


    
    
    void Update()
    {
        if (Time.time > _nextSpawn)
        {
            _nextSpawn = Time.time + 3f;
            Instantiate(_enemyPrefab, new Vector3(0, 11f, 0), Quaternion.identity);
            transform.Translate(Vector3.down * _enemyMovement * Time.deltaTime);
        }
        //bonus if at the bottom, respawn at new random top position
        else if (this.gameObject.transform.position.x < -11f)
        {
            Instantiate(_enemyPrefab, new Vector3(0, 11f, 0), Quaternion.identity);
            transform.Translate(Vector3.down * _enemyMovement * Time.deltaTime);
        }
    }
}
