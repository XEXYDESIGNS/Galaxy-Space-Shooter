using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyBeam : MonoBehaviour
{
    [SerializeField] private float _enemyMovement = 4f;
    private float _randomPos;
    private Vector3 _startPosition;

    private float _beamOffsetX;
    private float _beamOffsetY;

    private Player _player;
    
    private Animator _animator;

    [SerializeField] private AudioSource _explosionSound;
    [SerializeField] private GameObject _enemyBeamPrefab;

    private void Start()
    {
        _beamOffsetX = 0.017f;
        _beamOffsetY = -8.345f;
        
        _randomPos = Random.Range(-8f, 8f);

        _startPosition = transform.position;
        
        _player = GameObject.Find("Player").GetComponent<Player>();

        if (_player == null)
        {
            Debug.LogError("Player is null!");
        }

        _animator = GetComponent<Animator>();

        if (_animator == null)
        {
            Debug.LogError("Animator is null");
        }

        _explosionSound = GameObject.Find("Explosion_Sound").GetComponent<AudioSource>();

        if (_explosionSound == null)
        {
            Debug.LogError("Explosion Audio Source is Null");
        }
        
        _animator.SetTrigger("StartMoving");

        StartCoroutine(RandomFiringRoutine());
    }

    void Update()
    {
        Boundaries();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            if (other != null)
            {
                other.transform.GetComponent<Player>().Damage();
            }
            _animator.SetTrigger("OnEnemyDeath");
            _enemyMovement = 0;
            Destroy(GetComponent<Collider2D>());
            Destroy(gameObject, 2.8f);
            _explosionSound.Play();
        }
        
        if (other.CompareTag("Laser"))
        {
            Destroy(other.gameObject);
            _animator.SetTrigger("OnEnemyDeath");
            _enemyMovement = 0;
            Destroy(GetComponent<Collider2D>());
            Destroy(gameObject, 2.8f);
            _explosionSound.Play();

            if (_player != null)
            {
                _player.ChangeScore(Random.Range(4, 13));
            }
        }

        if (other.CompareTag("Missile"))
        {
            Destroy(other.gameObject);
            _animator.SetTrigger("OnEnemyDeath");
            _enemyMovement = 0;
            Destroy(GetComponent<Collider2D>());
            Destroy(gameObject, 2.8f);
            _explosionSound.Play();
            if (_player != null)
            {
                _player.ChangeScore(100);
            }
        }
    }

    private void Boundaries()
    {
        if (transform.position.x > 8f)
        {
            transform.position = new Vector3(transform.position.x * (-1f), transform.position.y, 0);
        }

        if (transform.position.x < -8f)
        {
            transform.position = new Vector3(transform.position.x * (-1f), transform.position.y, 0);
        }
        
        if (transform.position.y < -4f)
        {
            transform.position = new Vector3(_randomPos, 6f, 0);
        }
    }

    IEnumerator RandomFiringRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(2.0f, 4.0f));
            Instantiate(_enemyBeamPrefab, new Vector3((transform.position.x + _beamOffsetX), (transform.position.y + _beamOffsetY), 0), Quaternion.identity);
        }
    }
    
}
