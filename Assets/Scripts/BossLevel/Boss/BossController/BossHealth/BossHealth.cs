using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class BossHealth : MonoBehaviour
{
   
    public int maxHealth = 1000; // Salud máxima del boss
    public int currentHealth; // Salud actual del boss

    private void Start()
    {
        currentHealth = maxHealth; // Inicializar la salud actual con la máxima
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage; // Restar la cantidad de daño
        if (currentHealth <= 0)
        {
             SceneManager.LoadScene("WinScene"); 
        }
    }


}
