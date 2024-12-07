using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandDamageCollider : MonoBehaviour
{
      private TestEnergy playerScript; // Script del jugador para aplicar el daño
    private Animator bossAnimator; // Animator del boss para detectar qué animación está activa

    void Start()
    {
        // Obtener el Animator del boss al que pertenece esta mano
        bossAnimator = GetComponentInParent<Animator>();
    }

    // Método que se llama cuando el jugador entra en el collider de la mano
   private void OnTriggerEnter(Collider other)
{
    // Verifica que el objeto que entra sea el jugador
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
        // Obtener el estado actual de la animación
        AnimatorStateInfo stateInfo = bossAnimator.GetCurrentAnimatorStateInfo(0);
        
        // Comprobar si la animación "Smash" o "Clap_Hit" está en ejecución
        Debug.Log("Current animation: " + stateInfo.IsName("Smash"));
        
        if (stateInfo.IsName("Smash"))  // Si la animación "Smash" está activa
        {
            playerScript.TakeDamage(2);  // Daño 2 para Smash
            Debug.Log("Jugador recibe 2 de daño por Smash.");
        }
        else if (stateInfo.IsName("Clap_Hit"))  // Si la animación "Clap_Hit" está activa
        {
            playerScript.TakeDamage(1);  // Daño 1 para Clap_Hit
            Debug.Log("Jugador recibe 1 de daño por Clap_Hit.");
        }
    }
}
}