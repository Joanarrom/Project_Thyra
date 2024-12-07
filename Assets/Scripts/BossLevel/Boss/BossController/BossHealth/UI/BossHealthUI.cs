using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 


public class BossHealthUI : MonoBehaviour
{
    public BossHealth bossHealth; 
    public Slider healthBar; 

    void Start()
    {
        if (bossHealth == null)
        {
            Debug.LogError("BossHealth no está asignado en el script BossHealthUI.");
            return;
        }

        if (healthBar == null)
        {
            Debug.LogError("HealthBar no está asignado en el script BossHealthUI.");
            return;
        }

        
        healthBar.maxValue = bossHealth.maxHealth;
        healthBar.value = bossHealth.currentHealth;
    }

    void Update()
    {
        
        if (bossHealth != null)
        {
            healthBar.value = bossHealth.currentHealth;
        }
    }
}

