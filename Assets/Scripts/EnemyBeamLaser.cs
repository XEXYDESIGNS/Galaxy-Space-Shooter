using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBeamLaser : MonoBehaviour
{
    [SerializeField] private AudioSource _beamSound;
    [SerializeField] private AudioSource _explosionSound;

    [SerializeField] private float _beamSpeed = 15f;

    private void Start()
    {
        _beamSound = GameObject.Find("Laser_Sound").GetComponent<AudioSource>();

        if (_beamSound == null)
        {
            Debug.LogError("Laser Sound Audio Source is Null");
        }
        
        _explosionSound = GameObject.Find("Explosion_Sound").GetComponent<AudioSource>();

        if (_explosionSound == null)
        {
            Debug.LogError("Explosion Sound Audio Source is Null");
        }
    }

    private void Update()
    {
        transform.Translate(Vector3.down * _beamSpeed * Time.deltaTime);
        
        if (transform.position.y < -15f)
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
                Debug.Log("Hit Player with Enemy Laser Beam");
                other.transform.GetComponent<Player>().Damage();
                _explosionSound.Play();
            }
        }
    }
}