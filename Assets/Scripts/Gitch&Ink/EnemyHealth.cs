using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
   public int maxHealth = 100;
    private int currentHealth;

    // Propiedad corregida
    public int CurrentHealth 
    {
        get { return currentHealth; }
    }

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        
        if(currentHealth <= 0)
        {
            FindObjectOfType<ThirdPersonController>()?.OnEnemyKilled();
            Destroy(gameObject);
        }
    }
}
