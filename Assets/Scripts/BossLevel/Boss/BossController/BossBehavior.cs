using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehavior : MonoBehaviour
{
     public Transform player; // Transform del jugador
    public Animator animator; // Componente Animator del boss
    public float rotationSpeed = 5f; // Velocidad de rotación hacia el jugador
    public float attackCooldown = 2f; // Tiempo entre ataques

    private float attackTimer; // Temporizador para el intervalo de ataques

    void Start()
    {
        attackTimer = attackCooldown; // Inicializar el temporizador
    }

    void Update()
    {
        RotateTowardsPlayer(); // Rotación hacia el jugador

        // Contar el tiempo y ejecutar ataque cuando sea el momento
        attackTimer -= Time.deltaTime;

        if (attackTimer <= 0f)
        {
            PerformAttack(); // Ejecutar el ataque cuando el temporizador llegue a 0
            attackTimer = attackCooldown; // Reiniciar el temporizador para el siguiente ataque
        }
    }

    void RotateTowardsPlayer()
    {
        // Calcular la dirección hacia el jugador
        Vector3 directionToPlayer = player.position - transform.position;
        directionToPlayer.y = 0; // Ignorar la componente vertical para no inclinar el boss

        // Calcular la rotación hacia el jugador
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    void PerformAttack()
    {
        // Activar el parámetro de la animación con SetBool de manera aleatoria
        int attackIndex = Random.Range(0, 2);
        string attackBool = attackIndex == 0 ? "Clap_Hit" : "Smash";

        animator.SetBool(attackBool, true);  // Activar la animación seleccionada
        animator.SetBool("ReturnIdle", false); // Desactivar el retorno a Idle
        StartCoroutine(ResetAttackAnimation(attackBool)); // Espera un tiempo para devolver al estado Idle
    }

    private IEnumerator ResetAttackAnimation(string attackBool)
    {
        // Esperar el tiempo necesario para que la animación termine
        yield return new WaitForSeconds(1f);  // Ajusta este tiempo dependiendo de la duración de la animación
        animator.SetBool(attackBool, false); // Desactivar la animación
        animator.SetBool("ReturnIdle", true); // Regresar a Idle
    }
}
