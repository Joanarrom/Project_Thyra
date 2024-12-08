using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class BossHealth : MonoBehaviour
{
   
    public int maxHealth = 1000; 
    public int currentHealth; 

    private void Start()
    {
        currentHealth = maxHealth; 
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage; 
        if (currentHealth <= 0)
        {
             SceneManager.LoadScene("WinScene"); 
        }
    }


}
