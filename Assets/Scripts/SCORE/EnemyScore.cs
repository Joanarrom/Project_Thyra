using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScore : MonoBehaviour
{
     public int puntosPorMatar = 100; 

    public void Morir()
    {
       
        ScoreManager.Instance.AddScore(puntosPorMatar);

       
        Destroy(gameObject);
    }
}
