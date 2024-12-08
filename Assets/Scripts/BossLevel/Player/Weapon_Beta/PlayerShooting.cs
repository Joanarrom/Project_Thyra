using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform shootingPoint;  //  punto desde donde el proyectil sera disparado (Asignar Empty GameObject)
    public float shootDelay = 0.8f;

    private float nextShootTime = 0f;

    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0) && Time.time >= nextShootTime) //Al pulsar el click izq del mouse dispara un proyectil
        {
            nextShootTime = Time.time + shootDelay;
            ShootProjectile(shootingPoint.forward);  // Disparo normal
        }

        
    }

    void ShootProjectile(Vector3 direction) //Instancia y dispara un proyectil cargado
    {
        GameObject projectile = Instantiate(projectilePrefab, shootingPoint.position, Quaternion.identity);
        projectile.GetComponent<Projectile>().shootingDirection = direction;  // Pasa la direccion correcta al proyectil
    }

    
}
