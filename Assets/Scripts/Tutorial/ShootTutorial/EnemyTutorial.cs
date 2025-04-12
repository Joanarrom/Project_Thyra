using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class EnemyTutorial : MonoBehaviour
{   
   public int puntosPorMatar = 100; 
    void OnTriggerEnter(Collider other)
    {
        
        Destroy(gameObject);

         ScoreManager.Instance.AddScore(puntosPorMatar);
    }
}
