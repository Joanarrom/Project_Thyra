using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PruebaMovimientoAI : MonoBehaviour
{public Transform player;               // Referencia al jugador
    public float detectionRange = 15f;     // Rango de detección del jugador
    public float followRange = 10f;        // Rango máximo para seguir al jugador
    public float stopDistance = 2f;        // Distancia mínima para detenerse al seguir al jugador
    public float attackRange = 3f;         // Rango de ataque
    public float attackCooldown = 1f;      // Tiempo entre ataques
    public int damageAmount = 20;          // Daño que inflige al jugador
    public float fieldOfView = 60f;        // Ángulo de visión del enemigo (en grados)

    private NavMeshAgent agent;
    private float lastAttackTime;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        lastAttackTime = -attackCooldown;
    }

    void Update()
    {
        // Calcular la distancia al jugador
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Verificar si el jugador está dentro del rango de detección y en el campo de visión
        if (distanceToPlayer <= detectionRange && IsPlayerInSight())
        {
            if (distanceToPlayer > stopDistance && distanceToPlayer <= followRange)
            {
                // Seguir al jugador manteniendo la distancia mínima
                agent.SetDestination(player.position);
            }
            else if (distanceToPlayer <= stopDistance)
            {
                // Si está dentro de la distancia mínima, detener el movimiento
                agent.ResetPath();
            }

            // Si está dentro del rango de ataque, intentar atacar
            if (distanceToPlayer <= attackRange)
            {
                AttackPlayer();
            }
        }
        else
        {
            // Si el jugador sale del rango de detección o no está en vista, detener el movimiento
            agent.ResetPath();
        }
    }

    // Método para verificar si el jugador está en el campo de visión y en línea de visión
    bool IsPlayerInSight()
    {
        // Dirección hacia el jugador
        Vector3 directionToPlayer = (player.position - transform.position).normalized;

        // Calcular el ángulo entre la dirección del enemigo y el jugador
        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

        // Verificar si el jugador está dentro del ángulo de visión
        if (angleToPlayer < fieldOfView / 2f)
        {
            // Realizar un raycast hacia el jugador para verificar si hay obstáculos
            if (Physics.Raycast(transform.position, directionToPlayer, out RaycastHit hit, detectionRange))
            {
                // Verificar si el objeto impactado por el raycast es el jugador
                if (hit.transform == player)
                {
                    return true; // El jugador está en vista y en rango
                }
            }
        }

        return false; // El jugador no está en vista o está bloqueado
    }

    void AttackPlayer()
    {
        // Verificar si ha pasado suficiente tiempo desde el último ataque
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            PlayerController playerController = player.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.TakeDamage(damageAmount);
                Debug.Log("El enemigo ataca al jugador infligiendo " + damageAmount + " de daño.");
            }

            lastAttackTime = Time.time;
        }
    }
}
