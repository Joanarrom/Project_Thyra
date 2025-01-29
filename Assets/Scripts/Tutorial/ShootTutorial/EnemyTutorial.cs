using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class EnemyTutorial : MonoBehaviour
{   
   
    void OnTriggerEnter(Collider other)
    {
        
        Destroy(gameObject);

        
    }
}
