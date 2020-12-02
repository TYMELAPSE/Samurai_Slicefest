using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnManager : MonoBehaviour
{
    public List<Enemy> enemyPool;
    public Transform[] spawnLocations;
    public List<SpriteRenderer> bloodSplatterContainer;
    public Transform outOfPlacePosition;

    private float _spawnTimer = 0f;
    [SerializeField] private float _spawnCooldownTime = 2f;
    [SerializeField] private float _bloodSplatterFadeTime = 1f;

    private void Update()
    {
        _spawnTimer -= Time.deltaTime;
        if (_spawnTimer <= 0)
        {
            _spawnTimer = _spawnCooldownTime;
            SpawnNewEnemy();
        }
    }

    private void SpawnNewEnemy()
    {
        if (enemyPool.Count > 0)
        {
            Enemy enemyToSpawn = enemyPool[0];
            enemyToSpawn.transform.position = spawnLocations[Random.Range(0, spawnLocations.Length)].position;
            enemyToSpawn.isActive = true;
            enemyToSpawn.ToggleCollider(true);
            enemyPool.RemoveAt(0);
        }
    }

    public void AddEnemyToPool(Enemy enemyToAddToPool)
    {
        enemyPool.Add(enemyToAddToPool);
        enemyToAddToPool.isActive = false;
        enemyToAddToPool.ToggleCollider(false);
        enemyToAddToPool.transform.position = outOfPlacePosition.position;
    }

    public void ResetSpawnTimer()
    {
        _spawnTimer = _spawnCooldownTime;
    }

    public void SpawnBlood(Transform spawnPosition, Quaternion rotation)
    {
        StartCoroutine(SpawnBloodSplatter(spawnPosition, rotation));
    }

    public IEnumerator SpawnBloodSplatter(Transform spawnPosition, Quaternion rotation)
    {
        if (bloodSplatterContainer.Count > 0)
        {
            var bloodSplatter = bloodSplatterContainer[0];
            bloodSplatterContainer.RemoveAt(0);
            bloodSplatter.transform.position = spawnPosition.transform.position;
            bloodSplatter.transform.rotation = rotation;
            LeanTween.alpha(bloodSplatter.gameObject, 0f, _bloodSplatterFadeTime);
            
            yield return new WaitForSeconds(_bloodSplatterFadeTime);
            bloodSplatterContainer.Add(bloodSplatter);
            bloodSplatter.transform.position = outOfPlacePosition.position;
            LeanTween.alpha(bloodSplatter.gameObject, 1f, 0.02f);
        }
    }
}
