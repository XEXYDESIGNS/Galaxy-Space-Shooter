using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private int startPositionY = -3;

    [SerializeField]
    private float _speed = 3.5f;

    [SerializeField]
    private GameObject _laserPrefab;
    
    [SerializeField]
    private float _fireRate = 0.5f;

    private float _nextFire = 0.0f;

    [SerializeField]
    private int _lives = 3;

    private SpawnManager _spawnManager; //find the gameobject. then get component
    void Start()
    {
        transform.position = new Vector3(0, startPositionY, 0);
        _spawnManager =
            GameObject.Find("SpawnManager").GetComponent<SpawnManager>(); //find the object. get the component

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

        float maxHeight = 5.9f;
        float minHeight = -3.8f;

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, minHeight, maxHeight), 0);


        float maxBoundary = 11f;
        float minBoundary = -11f;
        
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
        Vector3 _laserOffsetPosition = new Vector3(transform.position.x, transform.position.y + 1.0f, 0);
        
        _nextFire = Time.time + _fireRate;
        Instantiate(_laserPrefab, _laserOffsetPosition, Quaternion.identity);
        
    }

    public void Damage()
    {
        _lives--;

        if (_lives < 1)
        {
            //communicate with the spawn manager
            //let them know to stop spawning
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
        }
    }
}
