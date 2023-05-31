using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField] private float _powerupSpeed = 3;
    
    //ID for powerups
    //0 - triple shot
    //1 - speed
    //2 - shields

    [SerializeField]
    private int powerupID;

    [SerializeField] private AudioSource _powerupSound;

    private void Start()
    {
        _powerupSound = GameObject.Find("Powerup_Sound").GetComponent<AudioSource>();

        if (_powerupSound == null)
        {
            Debug.LogError("Powerup Audio Source is null");
        }
    }

    void Update()
    {
        transform.Translate(Vector3.down * _powerupSpeed * Time.deltaTime);
        
        if (transform.position.y < -4f)
        {
            Destroy(gameObject);
        }
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.transform.GetComponent<Player>();
            
            if (player != null)
            {
                switch (powerupID)
                {
                        case 0:
                        player.TripleShotActive();
                        break;
                        case 1:
                            player.SpeedBoostActive();
                            break;
                        case 2:
                            player.ShieldPowerupActive();
                            break;
                        default:
                            Debug.Log("Default value");
                            break;
                }
                _powerupSound.Play();
            }
            Destroy(gameObject);
        }
    }
}
