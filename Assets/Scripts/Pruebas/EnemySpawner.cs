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

    [Header("Destrucción con el Jugador")]
    public float timeToDestroy = 10f; // Tiempo en segundos que el jugador debe estar dentro de la zona
    private float timeInsideZone = 0f; // Temporizador para contar el tiempo que el jugador está dentro

    // Agregar un collider para detectar ataques
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))  // Detectamos cuando el jugador entra en contacto
        {
            // Cuando el jugador entra, empezamos a contar el tiempo
            timeInsideZone = 0f;  // Reiniciar el tiempo
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))  // Verificamos si el jugador sigue dentro
        {
            timeInsideZone += Time.deltaTime;  // Aumentamos el tiempo

            // Si el jugador lleva el tiempo suficiente dentro, destruimos el spawner
            if (timeInsideZone >= timeToDestroy)
            {
                SpawnerDestroyed();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))  // Detectamos cuando el jugador sale de la zona
        {
            // Si el jugador sale antes de completar el tiempo, reiniciamos el contador
            timeInsideZone = 0f;
        }
    }

    void Update()
    {
        if (isDestroyed) return; // Evita que el spawner siga funcionando si está destruido

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
        ai.Setup(this, patrolPoints, detectionRadius);  // Pasamos 3 parámetros
        activeEnemies.Add(enemy);
    }

    public void SpawnerDestroyed()
    {
        isDestroyed = true;

        // Detener el spawneo y destruir el objeto
        Destroy(gameObject);  // Esto destruirá el Spawner
    }
}
