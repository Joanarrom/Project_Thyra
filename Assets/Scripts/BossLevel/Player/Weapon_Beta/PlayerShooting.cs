using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform shootingPoint;  
    public float shootDelay = 0.8f;

    private float nextShootTime = 0f;

    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0) && Time.time >= nextShootTime)
        {
            nextShootTime = Time.time + shootDelay;
            ShootProjectile(shootingPoint.forward); 
        }

        
    }

    void ShootProjectile(Vector3 direction) 
    {
        GameObject projectile = Instantiate(projectilePrefab, shootingPoint.position, Quaternion.identity);
        projectile.GetComponent<Projectile>().shootingDirection = direction; 
    }

    
}
