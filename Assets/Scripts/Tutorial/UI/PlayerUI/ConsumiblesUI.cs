using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumiblesUI : MonoBehaviour
{
     public GameObject consumablesUI;  

    
    private void OnTriggerEnter(Collider other)
    {
       
        if (other.CompareTag("Player"))
        {
           
            if (consumablesUI != null)
            {
                consumablesUI.SetActive(true);
            }

           
            Debug.Log("Consumibles habilitados.");
        }
    }

}
