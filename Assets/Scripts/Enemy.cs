using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _enemyMovement = 4f;

    void Update()
    {
        transform.Translate(Vector3.down * _enemyMovement * Time.deltaTime);

        if (transform.position.y < -4f)
        {
            float randomX = Random.Range(-8f, 8f);
            transform.position = new Vector3(randomX, 6f, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Player player = other.transform.GetComponent<Player>();
        
        if(other.CompareTag("Player"))
        {
            if (player != null)
            {
                player.Damage();
            }
            Destroy(gameObject);
        }
        
        if (other.CompareTag("Laser"))
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
