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
            Destroy(gameObject); // Esto activará OnDestroy() en PowerSource.cs
        }
    }
}
