using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    
     public float speed = 20f; // Velocidad del proyectil
    private Rigidbody rb;

    // Dirección fija (en este caso, hacia adelante en el espacio mundial)
    public Vector3 shootingDirection = Vector3.forward;

    public int damage = 20; // Cantidad de daño que el proyectil inflige

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Obtener el Rigidbody del proyectil

        if (rb != null)
        {
            // Desactivamos la gravedad para evitar que el proyectil se desvíe hacia arriba o abajo
            rb.useGravity = false;

            // Aseguramos de que el proyectil se dispare en la dirección correcta
            Vector3 forwardDirection = shootingDirection;

            // Aplicamos la velocidad al Rigidbody en la dirección deseada
            rb.velocity = forwardDirection * speed;
        }
        else
        {
            Debug.LogWarning("No se encontró Rigidbody en el proyectil.");
        }

        // Destruir el proyectil después de un tiempo (si no ha colisionado)
        Destroy(gameObject, 3f); 
    }

    // Cambiar OnCollisionEnter a OnTriggerEnter
    void OnTriggerEnter(Collider other)
    {
        // Comprobar si el objeto tiene el tag "Enemy"
        if (other.CompareTag("EnemyBody"))
        {
            Debug.Log("El proyectil impactó a un enemigo.");

            // Intentar obtener el script BossHealth
            BossHealth bossHealth = other.GetComponent<BossHealth>();

            if (bossHealth != null)
            {
                Debug.Log("El proyectil hizo daño al jefe.");
                bossHealth.TakeDamage(damage); // Hacer daño al jefe
            }
        }

        
    }
}


