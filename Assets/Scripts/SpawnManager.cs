using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject _enemyContainer;

    [SerializeField] private GameObject[] _powerups;

    private bool _stopSpawning = false;
    
    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(1.5f);
        while (_stopSpawning == false)
        {
            Vector3 spawnPosition = new Vector3(Random.Range(-8f, 8f), 6f, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab, spawnPosition, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(5.0f);
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
        Vector3 spawnPosition = new Vector3(Random.Range(-8f, 8f), 6f, 0);
        Instantiate(_powerups[3], spawnPosition, Quaternion.identity);
        yield return new WaitForSeconds(7.0f);
    }

    public void SpawnHealthPowerup()
    {
        StartCoroutine(SpawnHealthPowerupRoutine());
    }

    IEnumerator SpawnHealthPowerupRoutine()
    {
        yield return new WaitForSeconds(7.0f);
        Vector3 spawnPosition = new Vector3(Random.Range(-8f, 8f), 6f, 0);
        Instantiate(_powerups[4], spawnPosition, Quaternion.identity);
        yield return new WaitForSeconds(7.0f);
    }

    IEnumerator SpawnRarePowerupRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(30.0f, 45.0f));
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
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
        StartCoroutine(SpawnRarePowerupRoutine());
    }
}
