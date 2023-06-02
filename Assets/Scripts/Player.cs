using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private int _startPositionY = -3;

    private float _speed = 3.5f;
    private float _newSpeed = 3f;

    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private GameObject _tripleShotPrefab;
    [SerializeField] private GameObject _speedPrefab;
    [SerializeField] private GameObject _shieldsPrefab;
    [SerializeField] private GameObject _ammoPrefab;
    [SerializeField] private GameObject _healthPrefab;
    [SerializeField] private int _shieldHits = 3;
    [SerializeField] private GameObject _misslePrefab;

    [SerializeField] private GameObject _visualShields;

    private float _fireRate = 0.5f;

    private float _nextFire = 0.0f;

    [SerializeField] private int _ammoCount;

    [SerializeField] private int _lives = 3;

    [SerializeField] private int _score = 0;

    float maxHeight = 5.65f;
    float minHeight = -3.75f;
    float maxBoundary = 11f;
    float minBoundary = -11f;

    private Vector3 _laserOffsetPosition;
    private Vector3 _missileOffsetPosition;

    [SerializeField] private bool _isTripleShotActive;
    [SerializeField] private bool _isSpeedActive;
    [SerializeField] private bool _isShieldsActive;
    [SerializeField] private bool _isMissileActive;

    [SerializeField] private SpawnManager _spawnManager;
    [SerializeField] private UI_Manager _uiManager;
    
    [SerializeField] private GameObject _rightEngine;
    [SerializeField] private GameObject _leftEngine;
    
    private AudioSource _laserShot;
    [SerializeField] private AudioSource _explosionSound;
    
    
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
        
        _laserShot = GameObject.Find("Laser_Sound").GetComponent<AudioSource>();

        if (_laserShot == null)
        {
            Debug.LogError("Laser Audio Source is null");
        }

        _explosionSound = GameObject.Find("Explosion_Sound").GetComponent<AudioSource>();

        if (_explosionSound == null)
        {
            Debug.LogError("Explosion Audio Source is null");
        }

        _ammoCount = 15;
    }
    
    void Update()
    {
        CalculateMovement();

        if (_isMissileActive == false)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                FireLaser();
            }
        }
        else if (_isMissileActive == true)
        {
            LaunchMissile();
        }
    }

    void CalculateMovement()
    {
        float horizonalMovement = Input.GetAxis("Horizontal");
        float verticalMovement = Input.GetAxis("Vertical");

        float leftArrowSpeed = 3.0f;

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            _speed = _speed + leftArrowSpeed;
        }
        else if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            _speed = _speed - leftArrowSpeed;
        }
        else
        {
            Vector3 direction = new Vector3(horizonalMovement, verticalMovement, 0);
            transform.Translate(direction * _speed * Time.deltaTime);
        }

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
        
        if (_ammoCount == 0)
        {
            _uiManager.ReloadingAmmo(true);
            _spawnManager.ReloadPowerup();
            return;
        }
        
        if (_isMissileActive == true)
        {
            
        }
        if ((Input.GetKey(KeyCode.Space)) && _isTripleShotActive)
        {
                UpdateAmmoCount(1);
                Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
                _laserShot.Play();
        }
        else
        {
                UpdateAmmoCount(1);
                Instantiate(_laserPrefab, _laserOffsetPosition, Quaternion.identity);
                _laserShot.Play();
        }
    }

    public void Damage()
    {
        if (_isShieldsActive == false)
        {
            _lives--;
            _uiManager.UpdateLives(_lives);
            
            if (_lives < 1)
            {
                _spawnManager.OnPlayerDeath();
                Destroy(this.gameObject);
                _explosionSound.Play();
            }
        }

        if (_isShieldsActive == true && _shieldHits == 3)
        {
            
            GameObject.Find("Shields").GetComponent<Renderer>().material.color = Color.green;
            _shieldHits--;
        }
        else if (_isShieldsActive == true && _shieldHits == 2)
        {
            GameObject.Find("Shields").GetComponent<Renderer>().material.color = Color.magenta;
            _shieldHits--;
        }
        else if (_isShieldsActive == true && _shieldHits == 1)
        {
            _isShieldsActive = false;
            _visualShields.SetActive(false);
            _shieldHits = 3;
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

    public void AmmoReloaded()
    {
        UpdateAmmoCount(-15);
        _uiManager.ReloadingAmmo(false);
    }
    
    public void ChangeScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }

    public void UpdateAmmoCount(int count)
    {
        _ammoCount -= count;
        _uiManager.NewAmmoCount(_ammoCount);
    }

    public void HealthChange()
    {
        _lives += 1;
        _uiManager.UpdateLives(_lives);
    }

    public void MissileActive()
    {
        _isMissileActive = true;
        _uiManager.MissileProgram(true);
    }

    public void LaunchMissile()
    {
        _missileOffsetPosition = new Vector3(transform.position.x, transform.position.y + 1.16f, 0);
        if (Input.GetKey(KeyCode.M))
        {
            _uiManager.MissileProgram(false);
            _isMissileActive = false;
            Instantiate(_misslePrefab, _missileOffsetPosition, Quaternion.identity);
        }
    }
}
