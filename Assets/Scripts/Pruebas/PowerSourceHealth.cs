using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerSourceHealth : MonoBehaviour
{ public int maxHealth = 100;
    private int currentHealth;

    void Start() => currentHealth = maxHealth;

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
              // Añadir puntuación si tiene EnemyScore
         EnemyScore score = GetComponent<EnemyScore>();
         if (score != null)
         {
            score.Morir(); // Esto suma puntos y destruye el objeto
         }
         else
         {
            Destroy(gameObject); // Fallback por si no tiene EnemyScore
         }
        }
    }
}
