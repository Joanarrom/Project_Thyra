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

    // Eliminar enemigos muertos de la lista
    activeEnemies.RemoveAll(e => e == null);

    // Solo contar el tiempo si todavía se puede spawnear
    if (activeEnemies.Count < maxEnemies)
    {
        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnInterval)
        {
            SpawnEnemy();
            spawnTimer = 0f;
        }
    }

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
