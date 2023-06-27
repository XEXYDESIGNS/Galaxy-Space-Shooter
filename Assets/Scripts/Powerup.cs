using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Powerup : MonoBehaviour
{
    [SerializeField] private float _powerupSpeed = 3;
    
    //ID for powerups
    //0 - triple shot
    //1 - speed
    //2 - shields
    //3 - ammo
    //4 - health
    //5 - missile
    //6 - death

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
            if (powerupID == 3)
            {
                transform.position = new Vector3(Random.Range(-8.0f, 8.0f), 6f, 0);
            }
            else
            {
                Destroy(gameObject);
            }
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
                        case 3:
                            player.AmmoReloaded();
                            break;
                        case 4:
                            player.HealthChange();
                            break;
                        case 5:
                            player.MissileActive();
                            break;
                        case 6:
                            player.DeathPowerup();
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
