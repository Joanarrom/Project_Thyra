using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandDamageCollider : MonoBehaviour
{
      private TestEnergy playerScript; // Script del jugador para aplicar el daño
    private Animator bossAnimator; 

    void Start()
    {
        // Se obtiene el Animator del boss 
        bossAnimator = GetComponentInParent<Animator>();
    }

  
   private void OnTriggerEnter(Collider other)  // Se ejecuta cuando el jugador entra en el collider de la mano
{
    // Verificar que el objeto tenga el Tag Player
    if (other.CompareTag("Player"))
    {
        playerScript = other.GetComponent<TestEnergy>(); // Obtiene el script del jugador

        if (playerScript != null)
        {
            Debug.Log("Jugador detectado.");
        }
    }
}

private void OnTriggerStay(Collider other)
{
   if (other.CompareTag("Player") && playerScript != null)
    {
        // Obtener el State actual del animator
        AnimatorStateInfo stateInfo = bossAnimator.GetCurrentAnimatorStateInfo(0);
        
        // Comprobar si el State es "Smash" o "Clap_Hit" 
        Debug.Log("Current animation: " + stateInfo.IsName("Smash"));
        
        if (stateInfo.IsName("Smash"))  
        {
            playerScript.TakeDamage(2);  
            Debug.Log("Jugador recibe 2 de daño por Smash.");
        }
        else if (stateInfo.IsName("Clap_Hit"))  
        {
            playerScript.TakeDamage(1);  
            Debug.Log("Jugador recibe 1 de daño por Clap_Hit.");
        }
    }
}
}