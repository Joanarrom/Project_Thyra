using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableUI : MonoBehaviour
{
   public GameObject sliderVida;    
    public GameObject sliderEnergia; 
   

    void Start()
    {
       
        sliderVida.SetActive(false); 
        sliderEnergia.SetActive(false); 
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            sliderVida.SetActive(true);    
            sliderEnergia.SetActive(true); 
        }
    }

   
}
