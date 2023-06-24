using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBeamLaser : MonoBehaviour
{
    [SerializeField] private AudioSource _beamSound;
    [SerializeField] private AudioSource _explosionSound;
    [SerializeField] private GameObject _enemyLaserBeam;
    
    private void Start()
    {
        _beamSound = GameObject.Find("Explosion_Sound").GetComponent<AudioSource>();

        if (_beamSound == null)
        {
            Debug.LogError("Explosion Audio Source is Null");
        }
        
        _enemyLaserBeam.SetActive(false);
        
        StartCoroutine(InitiateLaserBeamRoutine());
    }

    private void Update()
    {
        /*transform.Translate(Vector3.down * _laserSpeed * Time.deltaTime);
        
        if (transform.position.y < -4f)
        {
            if (transform.parent == true)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
        */
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

    IEnumerator InitiateLaserBeamRoutine()
    {
        yield return new WaitForSeconds(3.0f);
        _enemyLaserBeam.SetActive(true);
        _beamSound.Play();
        yield return new WaitForSeconds(1.0f);
        _enemyLaserBeam.SetActive(false);
        yield return new WaitForSeconds(7.0f);
        
    }
}