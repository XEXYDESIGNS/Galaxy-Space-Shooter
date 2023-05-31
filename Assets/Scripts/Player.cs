using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private int _startPositionY = -3;

    [SerializeField] private float _speed = 3.5f;
    [SerializeField] private float _newSpeed = 3f;

    [SerializeField] private GameObject _laserPrefab;

    [SerializeField] private GameObject _tripleShotPrefab;
    [SerializeField] private GameObject _speedPrefab;
    [SerializeField] private GameObject _shieldsPrefab;
    
    [SerializeField] private GameObject _visualShields;
    
    [SerializeField] private float _fireRate = 0.5f;

    private float _nextFire = 0.0f;

    [SerializeField] private int _lives = 3;

    [SerializeField] private int _score = 0;
    
    float maxHeight = 5.65f;
    float minHeight = -3.75f;
    float maxBoundary = 11f;
    float minBoundary = -11f;

    private Vector3 _laserOffsetPosition;
    
    [SerializeField] private bool _isTripleShotActive;
    [SerializeField] private bool _isSpeedActive;
    [SerializeField] private bool _isShieldsActive;

    private SpawnManager _spawnManager;
    [SerializeField]
    private UI_Manager _uiManager;
    
    [SerializeField] private GameObject _rightEngine;
    [SerializeField] private GameObject _leftEngine;
    
    
    void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UI_Manager>();

        if (_uiManager == null)
        {
            Debug.LogError("The UI_Manager is null.");
        }
        
        _visualShields.SetActive(false);
        
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
        if (_isShieldsActive == true)
        {
            _isShieldsActive = false;
            _visualShields.SetActive(false);
            return;
        }

        _lives--;

        if (_lives == 2)
        {
            _rightEngine.SetActive(true);
        }
        else if (_lives == 1)
        {
            _leftEngine.SetActive(true);
        }
        
        _uiManager.UpdateLives(_lives);

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

    public void SpeedBoostActive()
    {
        _isSpeedActive = true;
        _speed = _speed + _newSpeed;
        StartCoroutine(SpeedBoostCoolDownRoutine());
    }

    IEnumerator SpeedBoostCoolDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isSpeedActive = false;
        _speed = _speed - _newSpeed;
    }

    public void ShieldPowerupActive()
    {
        _isShieldsActive = true;
        _visualShields.SetActive(true);
    }
    
    public void ChangeScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }
}
