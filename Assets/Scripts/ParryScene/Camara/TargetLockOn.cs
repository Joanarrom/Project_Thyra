using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetLockOn : MonoBehaviour
{
    public Transform player; // El jugador
    public Camera mainCamera; // La cámara principal
    public float lockOnDistance = 20f; // Distancia máxima para fijar un objetivo
    public LayerMask targetLayer; // La capa donde están los objetivos fijables
    public float smoothTime = 0.2f; // Suavidad de la transición al fijar un objetivo

    private Transform currentTarget; // Objetivo actual fijado
    private Vector3 currentVelocity; // Para suavizar el movimiento de la cámara

    void Update()
    {
        // Alternar el objetivo cuando se presiona la tecla
        if (Input.GetKeyDown(KeyCode.Tab)) 
        {
            if (currentTarget != null)
            {
                // Si ya hay un objetivo fijado, lo soltamos
                currentTarget = null;
            }
            else
            {
                // Si no hay objetivo, buscar uno
                FindClosestTarget();
            }
        }

        // Rotar la cámara para mirar al objetivo fijado
        if (currentTarget != null)
        {
            LockOnTarget();
        }
    }

    void FindClosestTarget()
    {
        // Encontrar todos los objetos en el rango
        Collider[] targetsInRange = Physics.OverlapSphere(player.position, lockOnDistance, targetLayer);

        float closestDistance = Mathf.Infinity;
        Transform closestTarget = null;

        // Buscar el más cercano
        foreach (var target in targetsInRange)
        {
            float distanceToTarget = Vector3.Distance(player.position, target.transform.position);

            if (distanceToTarget < closestDistance)
            {
                closestDistance = distanceToTarget;
                closestTarget = target.transform;
            }
        }

        // Asignar el objetivo más cercano
        currentTarget = closestTarget;
    }

    void LockOnTarget()
    {
        // Calcular la dirección hacia el objetivo
        Vector3 directionToTarget = currentTarget.position - player.position;

        // Girar el jugador hacia el objetivo
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
        player.rotation = Quaternion.Lerp(player.rotation, targetRotation, smoothTime);

        // Ajustar la cámara para mirar al objetivo
        Vector3 targetPosition = currentTarget.position;
        Vector3 cameraPosition = Vector3.SmoothDamp(mainCamera.transform.position, targetPosition, ref currentVelocity, smoothTime);

        // Mover la cámara hacia el objetivo
        mainCamera.transform.LookAt(targetPosition);
    }
}
