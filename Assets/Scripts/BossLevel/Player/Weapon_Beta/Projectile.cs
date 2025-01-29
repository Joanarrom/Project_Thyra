using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    
     public float speed = 20f; 
    private Rigidbody rb;

    public Vector3 shootingDirection = Vector3.forward;

    public int damage = 30; 

    void Start()
    {
        rb = GetComponent<Rigidbody>(); 

        if (rb != null)
        {
           
            rb.useGravity = false;

            
            Vector3 forwardDirection = shootingDirection;

           
            rb.velocity = forwardDirection * speed;
        }
        else
        {
            Debug.LogWarning("No se encontró Rigidbody en el proyectil.");
        }

        
        Destroy(gameObject, 3f); 
    }

    
    void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("EnemyBody"))
        {
            Debug.Log("El proyectil impactó a un enemigo.");

           
            BossHealth bossHealth = other.GetComponent<BossHealth>();

            if (bossHealth != null)
            {
                Debug.Log("El proyectil hizo daño al jefe.");
                bossHealth.TakeDamage(damage); 
            }
        }

        
    }
}


