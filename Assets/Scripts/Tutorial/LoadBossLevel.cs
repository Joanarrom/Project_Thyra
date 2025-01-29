using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LoadBossLevel : MonoBehaviour
{
     public string sceneToLoad = "BossLevel"; 

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Player"))
        {
           
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
