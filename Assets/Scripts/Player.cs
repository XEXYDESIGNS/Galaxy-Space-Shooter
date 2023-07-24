using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private int _startPositionY = -3;

    [SerializeField] private float _speed = 3.5f;
    [SerializeField] private float _newSpeed = 3f;

    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private GameObject _tripleShotPrefab;
    [SerializeField] private GameObject _speedPrefab;
    [SerializeField] private GameObject _shieldsPrefab;
    [SerializeField] private GameObject _ammoPrefab;
    [SerializeField] private GameObject _healthPrefab;
    [SerializeField] private int _shieldHits = 3;
    [SerializeField] private GameObject _misslePrefab;

    [SerializeField] private GameObject _visualShields;
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private Transform _cameraTransform;

    [SerializeField] private Vector3 _originalPosition;

    [SerializeField] private Slider _thrusterChargeSlider;

    private float _fireRate = 0.5f;

    private float _nextFire = 0.0f;

    [SerializeField] private int _ammoCount;

    [SerializeField] private int _lives = 3;

    [SerializeField] private int _score = 0;

    float _maxHeight = 5.65f;
    float _minHeight = -3.75f;
    float _maxBoundary = 11f;
    float _minBoundary = -11f;

    [SerializeField] private float _thrusterCount;
    [SerializeField] private GameObject _thrustersVisual;

    private Vector3 _laserOffsetPosition;
    private Vector3 _missileOffsetPosition;

    [SerializeField] private bool _isTripleShotActive;
    [SerializeField] private bool _isSpeedActive;
    [SerializeField] private bool _isShieldsActive;
    [SerializeField] private bool _isMissileActive;
    [SerializeField] private bool _hasReloadHappened;

    [SerializeField] private SpawnManager _spawnManager;
    [SerializeField] private UI_Manager _uiManager;
    
    [SerializeField] private GameObject _rightEngine;
    [SerializeField] private GameObject _leftEngine;
    
    private AudioSource _laserShot;
    [SerializeField] private AudioSource _explosionSound;

    [SerializeField] private float _currentXPos; 
    
    
    void Start()
    {
        if (_cameraTransform == null)
        {
            Debug.LogError("Camera transform is null");
        }

        _originalPosition = _cameraTransform.localPosition;
        
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
        _isSpeedActive = false;
    }
    
    void Update()
    {
        if(_thrusterCount < 1)
        {
            CalculateMovement();
        }
        else
        {
            _thrustersVisual.SetActive(false);
            StartCoroutine(MaxThrusterCountCoolDown());
        }

        if(_ammoCount > 0)
        {
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
        
        if(_ammoCount < 1 && _hasReloadHappened == false)
        {
            _hasReloadHappened = true;
            _uiManager.ReloadingAmmo(true);
            _spawnManager.ReloadPowerup();
        }
        
        MyCurrentXPos();
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

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveThrusterSlider();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveThrusterSlider();
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            MoveThrusterSlider();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            MoveThrusterSlider();
        }

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, _minHeight, _maxHeight), 0);

        if (transform.position.x > _maxBoundary)
        {
            transform.position = new Vector3(_minBoundary, transform.position.y, 0);
        }
        else if (transform.position.x < _minBoundary)
        {
            transform.position = new Vector3(_maxBoundary, transform.position.y, 0);
        }
    }

    void FireLaser()
    {
        _laserOffsetPosition = new Vector3(transform.position.x, transform.position.y + 1.0f, 0);
        _nextFire = Time.time + _fireRate;
        
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
        StartCoroutine(CameraShakeRoutine());
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
        _speed -= _newSpeed;
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
        _hasReloadHappened = false;
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

    public void DeathPowerup()
    {
        _lives -= 1;
        _uiManager.UpdateLives(_lives);
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

    public void MoveThrusterSlider()
    {
        _thrusterCount += 0.05f;
        _uiManager.ChangeThrusterSlider(_thrusterCount);
    }

    public void MyCurrentXPos()
    {
        _currentXPos = transform.position.x;
        
    }

    IEnumerator MaxThrusterCountCoolDown()
    {
        yield return new WaitForSeconds(3.5f);
        _thrusterChargeSlider.value = 0;
        _thrusterCount = 0;
        _thrustersVisual.SetActive(true);
    }

    IEnumerator CameraShakeRoutine()
    {
        yield return new WaitForSeconds(0.1f);
        _mainCamera.transform.Translate(transform.position.x + 0.5f, transform.position.y, transform.position.z);
        yield return new WaitForSeconds(0.1f);
        _mainCamera.transform.localPosition = _originalPosition;
        yield return new WaitForSeconds(0.1f);
        _mainCamera.transform.Translate(transform.position.x - 0.5f, transform.position.y, transform.position.z);
        yield return new WaitForSeconds(0.1f);
        _mainCamera.transform.localPosition = _originalPosition;
        yield return new WaitForSeconds(0.1f);
        _mainCamera.transform.Translate(transform.position.x - 0.5f, transform.position.y, transform.position.z);
        yield return new WaitForSeconds(0.1f);
        _mainCamera.transform.localPosition = _originalPosition;
        yield return new WaitForSeconds(0.1f);
    }
}
