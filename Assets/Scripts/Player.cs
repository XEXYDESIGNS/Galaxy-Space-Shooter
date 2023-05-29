using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private int _startPositionY = -3;

    [SerializeField] private float _speed = 3.5f;

    [SerializeField] private GameObject _laserPrefab;

    [SerializeField] private GameObject _tripleShotPrefab;
    
    [SerializeField] private float _fireRate = 0.5f;

    private float _nextFire = 0.0f;

    [SerializeField] private int _lives = 3;
    
    float maxHeight = 5.65f;
    float minHeight = -3.75f;
    float maxBoundary = 11f;
    float minBoundary = -11f;

    private Vector3 _laserOffsetPosition;
    
    [SerializeField] private bool _isTripleShotActive;
    
    private SpawnManager _spawnManager;
    
    void Start()
    {
        transform.position = new Vector3(0, _startPositionY, 0);
        _spawnManager =
            GameObject.Find("SpawnManager").GetComponent<SpawnManager>();

        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is null.");
        }
    }
    
    void Update()
    {
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _nextFire)
        {
            FireLaser();
        }
    }

    void CalculateMovement()
    {
        float horizonalMovement = Input.GetAxis("Horizontal");
        float verticalMovement = Input.GetAxis("Vertical");
        

        Vector3 direction = new Vector3(horizonalMovement, verticalMovement, 0);
        transform.Translate(direction * _speed * Time.deltaTime);

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, minHeight, maxHeight), 0);

        if (transform.position.x > maxBoundary)
        {
            transform.position = new Vector3(minBoundary, transform.position.y, 0);
        }
        else if (transform.position.x < minBoundary)
        {
            transform.position = new Vector3(maxBoundary, transform.position.y, 0);
        }
    }

    void FireLaser()
    {
        _laserOffsetPosition = new Vector3(transform.position.x, transform.position.y + 1.0f, 0);
        _nextFire = Time.time + _fireRate;
        
        if ((Input.GetKey(KeyCode.Space)) && _isTripleShotActive)
        {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, _laserOffsetPosition, Quaternion.identity);
        }
    }

    public void Damage()
    {
        _lives--;

        if (_lives < 1)
        {
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
        }
    }

    public void TripleShotActive()
    {
        _isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isTripleShotActive = false;
    }
}
