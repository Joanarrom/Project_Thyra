using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public int maxEnemies = 4;
    public float spawnInterval = 5f;
    public Transform[] patrolPoints;
    public float detectionRadius = 10f;

    private List<GameObject> activeEnemies = new List<GameObject>();
    private float spawnTimer;
    private bool isDestroyed = false;

    void Update()
    {
        if (isDestroyed) return;

        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnInterval && activeEnemies.Count < maxEnemies)
        {
            SpawnEnemy();
            spawnTimer = 0f;
        }

        activeEnemies.RemoveAll(e => e == null);
    }

    void SpawnEnemy()
    {
        GameObject enemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
        EnemyAI ai = enemy.GetComponent<EnemyAI>();
        ai.Setup(this, patrolPoints, detectionRadius);
        activeEnemies.Add(enemy);
    }

    public void SpawnerDestroyed()
    {
        isDestroyed = true;
    }
}
