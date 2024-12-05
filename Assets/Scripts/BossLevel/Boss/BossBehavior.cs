using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehavior : MonoBehaviour
{
    public Transform player; // Transform del jugador
    public Animator animator; // Componente Animator del boss
    public float rotationSpeed = 5f; // Velocidad de rotación hacia el jugador
    public float rotationThreshold = 1f; // Umbral para considerar que el boss está "fijado" al jugador
    public float attackRange = 3f; // Rango de ataque del boss
    public int attackDamage = 10; // Daño del ataque del boss

    private bool isPlayerDetected = false; // Para saber si el jugador está en el área de detección
    private bool isAttacking = false; // Para saber si el boss está atacando

    // Referencia al script del jugador
    private TestEnergy playerScript;

    // Referencias a los colliders de los brazos (ataque)
    public Collider leftArmCollider; // Collider del brazo izquierdo (para el ataque)
    public Collider rightArmCollider; // Collider del brazo derecho (para el ataque)

    void Start()
    {
        playerScript = player.GetComponent<TestEnergy>(); // Obtenemos el script del jugador

        // Desactivar los colliders de los brazos al principio
        leftArmCollider.enabled = false;
        rightArmCollider.enabled = false;
    }

    void Update()
    {
        if (isPlayerDetected && !isAttacking)
        {
            RotateTowardsPlayer();
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

        // Verificar si el jugador está dentro del rango de ataque
        float distanceToPlayer = directionToPlayer.magnitude;

        // Si el jugador está dentro del rango de ataque y el boss está alineado con él, lanzar un ataque
        if (distanceToPlayer <= attackRange && Quaternion.Angle(transform.rotation, targetRotation) < rotationThreshold && !isAttacking)
        {
            StartCoroutine(PerformAttack());
        }
    }

    // Corutina para realizar el ataque
    System.Collections.IEnumerator PerformAttack()
    {
        isAttacking = true;

        // Activar los colliders de los brazos para que puedan hacer daño
        leftArmCollider.enabled = true;
        rightArmCollider.enabled = true;

        // Elegir aleatoriamente un ataque
        int attackIndex = Random.Range(0, 2);
        string attackTrigger = attackIndex == 0 ? "Attack1" : "Attack2";

        // Activar el trigger de animación de ataque
        animator.SetTrigger(attackTrigger);
        Debug.Log("Activando animación de ataque");

        // Esperar hasta que la animación termine (esto puede ser diferente dependiendo del ataque)
        yield return new WaitForSeconds(1f); // Ajustar el tiempo dependiendo de la animación

        // Después de realizar el ataque, desactivar los colliders de los brazos
        leftArmCollider.enabled = false;
        rightArmCollider.enabled = false;

        // Regresar a la animación Idle
        animator.SetTrigger("Idle");
        Debug.Log("Volviendo a la animación Idle");

        // Finalizar el ataque
        isAttacking = false;
    }

    // Detectar cuando el jugador entra en el área de detección
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Asegúrate de que el jugador tiene el tag "Player"
        {
            Debug.Log("Jugador detectado.");
            isPlayerDetected = true;
        }
    }

    // Detectar cuando el jugador sale del área de detección
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Jugador fuera del área de detección.");
            isPlayerDetected = false;
        }
    }

    // Detectar cuando el jugador entra en el área de los colliders de los brazos durante el ataque
    private void OnTriggerStay(Collider other)
    {
        if ((other.CompareTag("Player") && (other == leftArmCollider || other == rightArmCollider)) && isAttacking)
        {
            ApplyDamageToPlayer(other);  // Aplicar daño si está dentro del rango de ataque de los brazos
        }
    }

    // Aplicar daño al jugador
    private void ApplyDamageToPlayer(Collider playerCollider)
    {
        if (playerScript != null)
        {
            // Solo aplica el daño si el jugador está dentro del área de impacto
            playerScript.TakeDamage(attackDamage);
            Debug.Log($"El jugador recibió {attackDamage} puntos de daño.");
        }
    }
}