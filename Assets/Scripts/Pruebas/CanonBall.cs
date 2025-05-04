using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonBall : MonoBehaviour
{
 public int damage = 20;
    public float lifetime = 5f;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ThirdPersonController player = other.GetComponent<ThirdPersonController>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }

            Destroy(gameObject); // Destruye la bala al impactar
        }
        else if (!other.isTrigger)
        {
            // Si choca con otra cosa que no sea trigger (suelo, paredes), destruye la bala
            Destroy(gameObject);
        }
    }
}
