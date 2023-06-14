using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject _enemyContainer;

    [SerializeField] private GameObject[] _powerups;
    [SerializeField] private bool _isReloadAmmoActive;

    private bool _stopSpawning = false;

    [SerializeField] private int _enemyWaveCount;
    [SerializeField] private UI_Manager _uiManager;


    private void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UI_Manager>();

        if (_uiManager == null)
        {
            Debug.LogError("The UI_Manager is null.");
        }
    }

    IEnumerator SpawnEnemyRoutineWave1()
    {
        yield return new WaitForSeconds(1.5f);
        while (_enemyWaveCount < 5)
        {
            _enemyWaveCount += 1;
            Vector3 spawnPosition = new Vector3(Random.Range(-8f, 8f), 6f, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab, spawnPosition, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            _uiManager.UpdateEnemyWave(_enemyWaveCount, 5);
            _uiManager.UpdateWave(1);
            if (_enemyWaveCount > 4)
            {
                yield return StartCoroutine(SpawnEnemyRoutineWave2());
            }
            yield return new WaitForSeconds(5.0f);
        }
    }

    IEnumerator SpawnEnemyRoutineWave2()
    {
        yield return new WaitForSeconds(1.5f);
        while (_enemyWaveCount > 4 && _enemyWaveCount < 15)
        {
            _enemyWaveCount += 1;
            Vector3 spawnPosition = new Vector3(Random.Range(-8f, 8f), 6f, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab, spawnPosition, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            _uiManager.UpdateEnemyWave(_enemyWaveCount - 5, 10);
            _uiManager.UpdateWave(2);
            yield return new WaitForSeconds(4.0f);
        }
    }

    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(1.5f);
        
        Vector3 spawnPosition = new Vector3(Random.Range(-8f, 8f), 6f, 0);
        while (_stopSpawning == false)
        {
            int randomPowerup = Random.Range(0, 3);
            Instantiate(_powerups[randomPowerup], spawnPosition, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(2, 8));
        }
    }

    public void ReloadPowerup()
    {
        StartCoroutine(ReloadPowerupRoutine());
    }

    IEnumerator ReloadPowerupRoutine()
    {
        while (!_isReloadAmmoActive)
        {
            _isReloadAmmoActive = true;
            yield return new WaitForSeconds(3.0f);
            Vector3 spawnPosition = new Vector3(Random.Range(-8f, 8f), 6f, 0);
            Instantiate(_powerups[3], spawnPosition, Quaternion.identity);
        }
    }

    public void SpawnHealthPowerup()
    {
        StartCoroutine(SpawnHealthPowerupRoutine());
    }

    IEnumerator SpawnHealthPowerupRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(7.0f);
            Vector3 spawnPosition = new Vector3(Random.Range(-8f, 8f), 6f, 0);
            Instantiate(_powerups[4], spawnPosition, Quaternion.identity);
        }
    }

    IEnumerator SpawnRarePowerupRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(35.0f, 45.0f));
            Vector3 spawnPosition = new Vector3(Random.Range(-8f, 8f), 6f, 0);
            Instantiate(_powerups[5], spawnPosition, Quaternion.identity);
        }
    }
    
    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutineWave1());
        StartCoroutine(SpawnPowerupRoutine());
        StartCoroutine(SpawnRarePowerupRoutine());
    }
}
