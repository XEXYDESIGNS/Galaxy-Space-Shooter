using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLasers : MonoBehaviour
{
    private float _laserSpeed = 4.0f;
    
    [SerializeField] private AudioSource _explosionSound;
    
    private void Start()
    {
        _explosionSound = GameObject.Find("Explosion_Sound").GetComponent<AudioSource>();

        if (_explosionSound == null)
        {
            Debug.LogError("Explosion Audio Source is Null");
        }
    }

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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (other != null)
            {
                Debug.Log("Hit Player with Enemy Laser");
                other.transform.GetComponent<Player>().Damage();
                _explosionSound.Play();
            }
        }
    }
}
