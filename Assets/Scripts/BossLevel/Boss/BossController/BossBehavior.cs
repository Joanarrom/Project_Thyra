using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehavior : MonoBehaviour
{
     public Transform player; 
    public Animator animator; 
    public float rotationSpeed = 5f; 
    public float attackCooldown = 2f; 

    private float attackTimer; // Timer del intervalo de ataques

    void Start()
    {
        attackTimer = attackCooldown; // Inicia el timer
    }

    void Update()
    {
        RotateTowardsPlayer();

        // Cuenta regresiva para el ataque
        attackTimer -= Time.deltaTime;

        if (attackTimer <= 0f)
        {
            PerformAttack(); // Ejecuta el ataque cuando el temporizador llegue a 0
            attackTimer = attackCooldown; // Reinicio del timer al llegar a 0
        }
    }

    void RotateTowardsPlayer()
    {
        // Calcular la dirección hacia el player
        Vector3 directionToPlayer = player.position - transform.position;
        directionToPlayer.y = 0; 

        // Calcular la rotación hacia el player
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    void PerformAttack()
    {
        // Activar un SetBool del animator de manera aleatoria
        int attackIndex = Random.Range(0, 2);
        string attackBool = attackIndex == 0 ? "Clap_Hit" : "Smash";

        animator.SetBool(attackBool, true);  
        animator.SetBool("ReturnIdle", false); 
        StartCoroutine(ResetAttackAnimation(attackBool)); // Espera un tiempo para devolver al estado Idle
    }

    private IEnumerator ResetAttackAnimation(string attackBool) //Reset a la animacion de Iddle
    {
        
        yield return new WaitForSeconds(1f);  
        animator.SetBool(attackBool, false); 
        animator.SetBool("ReturnIdle", true); 
    }
}
