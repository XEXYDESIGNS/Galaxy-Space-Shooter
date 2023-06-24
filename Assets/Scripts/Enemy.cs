using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _enemyMovement = 4f;
    private float _randomPos;

    private bool _enemyLaserActive;

    private Player _player;
    
    private Animator _animator;

    [SerializeField] private AudioSource _explosionSound;
    [SerializeField] private GameObject _enemyLasersPrefab;

    private void Start()
    {
        _randomPos = Random.Range(-8f, 8f);
        
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

        StartCoroutine(RandomFiringTime());
    }

    void Update()
    {
        StartCoroutine(EnemyMovementRoutine());
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
            _enemyLasersPrefab.SetActive(false);
            Destroy(gameObject, 2.8f);
            _explosionSound.Play();
        }
        
        if (other.CompareTag("Laser"))
        {
            Destroy(other.gameObject);
            _animator.SetTrigger("OnEnemyDeath");
            _enemyMovement = 0;
            Destroy(GetComponent<Collider2D>());
            _enemyLasersPrefab.SetActive(false);
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
            _enemyLasersPrefab.SetActive(false);
            Destroy(gameObject, 2.8f);
            _explosionSound.Play();
            if (_player != null)
            {
                _player.ChangeScore(100);
            }
        }
    }

    IEnumerator RandomFiringTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(2.0f, 4.0f));
            Instantiate(_enemyLasersPrefab, transform.position, Quaternion.identity);
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

    IEnumerator EnemyMovementRoutine()
    {
        transform.Translate(Vector3.left * _enemyMovement * Time.deltaTime);
        yield return new WaitForSeconds(1.5f);
        transform.Translate(Vector3.down * _enemyMovement * Time.deltaTime);
        yield return new WaitForSeconds(1.0f);
        transform.Translate(Vector3.left * _enemyMovement * Time.deltaTime);
        yield return new WaitForSeconds(5.0f);
        transform.Translate(Vector3.down * _enemyMovement * Time.deltaTime);
        yield return new WaitForSeconds(1.5f);
        transform.Translate(Vector3.right * _enemyMovement * Time.deltaTime);
        yield return new WaitForSeconds(1.0f);
        transform.Translate(Vector3.down * _enemyMovement * Time.deltaTime);
        yield return new WaitForSeconds(3.0f);
        transform.Translate(Vector3.left * _enemyMovement * Time.deltaTime);
        yield return new WaitForSeconds(5.0f);
        transform.Translate(Vector3.down * _enemyMovement * Time.deltaTime);
    }
}
