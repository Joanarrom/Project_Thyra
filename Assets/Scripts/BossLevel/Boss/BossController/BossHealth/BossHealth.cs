using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class BossHealth : MonoBehaviour
{
   
    public int maxHealth = 100; 
    public int currentHealth; 
    public int puntosPorMatar = 500; 

    private void Start()
    {
        currentHealth = maxHealth; 
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage; 
        if (currentHealth <= 0)
        {
             SceneManager.LoadScene("ScoreFinal"); 
               ScoreManager.Instance.AddScore(puntosPorMatar);
        }
    }


}
