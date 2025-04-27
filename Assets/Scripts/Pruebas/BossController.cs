using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BossController : MonoBehaviour
{
    public GameObject fireballPrefab;
    public GameObject impactMarkerPrefab;
    public float attackInterval = 5f;
    public float markerDuration = 1.5f;
    public int totalPowerSources = 4;
    public Transform target; // El jugador

    private float timer;
    private int destroyedSources = 0;

    void Start()
    {
        if (target == null)
            target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= attackInterval)
        {
            StartCoroutine(FireMeteor());
            timer = 0f;
        }
    }

    IEnumerator FireMeteor()
    {
        Vector3 targetPos = target.position;
        Vector3 markerPos = new Vector3(targetPos.x, 0.1f, targetPos.z);

        GameObject marker = Instantiate(impactMarkerPrefab, markerPos, Quaternion.identity);
        yield return new WaitForSeconds(markerDuration);

        Vector3 spawnPos = targetPos + Vector3.up * 15f;
        Instantiate(fireballPrefab, spawnPos, Quaternion.identity);
        Destroy(marker, 2f);
    }

    public void PowerSourceDestroyed()
    {
        destroyedSources++;
        if (destroyedSources >= totalPowerSources)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Boss defeated!");
        Destroy(gameObject);
    }
}