﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    [SerializeField] private Text _scoreText;
    [SerializeField] private Image _livesImage;
    [SerializeField] private Sprite[] _liveSprites;
    [SerializeField] private Text _gameOverText;
    [SerializeField] private Text _restartGameText;
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private Text _ammoCountDisplay;
    [SerializeField] private Text _restartAmmoCount;
    [SerializeField] private GameObject _rightFire;
    [SerializeField] private GameObject _leftFire;
    [SerializeField] private SpawnManager _spawnManager;

    // Start is called before the first frame update
    void Start()
    {
        _scoreText.text = "Score: " + 0;
        _ammoCountDisplay.text = "Ammo Count -- " + 15;
        _gameOverText.gameObject.SetActive(false);
        _restartAmmoCount.gameObject.SetActive(false);
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();

        if (_gameManager == null)
        {
            Debug.LogError("Game Manager is null");
        }

        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();

        if (_spawnManager == null)
        {
            Debug.LogError("Spawn Manager is null");
        }
    }

    public void UpdateScore(int updateScore)
    {
        _scoreText.text = "Score: " + updateScore;
    }

    public void NewAmmoCount(int count)
    {
        _ammoCountDisplay.text = "Ammo Count -- " + count;
    }

    public void UpdateLives(int currentLives)
    {
        _livesImage.sprite = _liveSprites[currentLives];
        if (currentLives < 3)
        {
            {
                _spawnManager.SpawnHealthPowerup();
            }

            if (currentLives == 3)
            {
                _rightFire.SetActive(false);
                _leftFire.SetActive(false);
            }
            else if (currentLives == 2)
            {
                _rightFire.SetActive(true);
                _leftFire.SetActive(false);
            }
            else if (currentLives == 1)
            {
                _rightFire.SetActive(true);
                _leftFire.SetActive(true);
            }

            if (currentLives == 0)
            {
                GameOverSequence();
            }
        }
    }

    void GameOverSequence()
        {
            _gameManager.GameOver();
            _gameOverText.gameObject.SetActive(true);
            _restartGameText.gameObject.SetActive(true);
            StartCoroutine(GameOverFlicker());
        }

        IEnumerator GameOverFlicker()
        {
            while (true)
            {
                yield return new WaitForSeconds(0.5f);
                _gameOverText.gameObject.SetActive(false);
                yield return new WaitForSeconds(0.5f);
                _gameOverText.gameObject.SetActive((true));
            }
        }

        public void ReloadingAmmo(bool _display)
        {
            _restartAmmoCount.gameObject.SetActive(_display);
        }
    }
