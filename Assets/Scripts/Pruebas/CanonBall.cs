using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonBall : MonoBehaviour
{
   public int damage = 20;
    public float lifetime = 5f;

    private void Start()
    {
        Destroy(gameObject, lifetime); // Destruye la bala tras un tiempo
    }

    private void OnTriggerEnter(Collider other)
    {
        // Si colisiona con el jugador
        if (other.CompareTag("Player"))
        {
            ThirdPersonController player = other.GetComponent<ThirdPersonController>();
            if (player != null)
            {
                player.TakeDamage(damage); // Aplica daño al jugador
            }
            Destroy(gameObject); // Destruye la bala tras impactar
        }
        // Si colisiona con cualquier otra cosa que NO sea el jugador
        else
        {
            Destroy(gameObject); // Destruye la bala
        }
    }
}
