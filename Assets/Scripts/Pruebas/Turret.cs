using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{ 
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 10f;
    public float detectionRange = 10f;
    public float fireRate = 1.5f;
    public float rotationSpeed = 5f;

    private Transform player;
    private float fireCooldown;
    private bool isShooting;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= detectionRange)
        {
            RotateTowardsPlayer();

            if (!isShooting && fireCooldown <= 0f)
            {
                StartCoroutine(Shoot());
                fireCooldown = fireRate;
            }
        }

        fireCooldown -= Time.deltaTime;
    }

    void RotateTowardsPlayer()
    {
          Vector3 direction = transform.position - player.position; // invertimos la dirección
         direction.y = 0f;
         Quaternion targetRotation = Quaternion.LookRotation(direction);
         transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }

    IEnumerator Shoot()
    {
        isShooting = true;

        yield return new WaitForSeconds(0.1f); // pequeño retraso antes de disparar

        Fire();

        isShooting = false;
    }

    void Fire()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Vector3 direction = (player.position - firePoint.position).normalized;
        bullet.GetComponent<Rigidbody>().velocity = direction * bulletSpeed;
    }
}
