using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFollowPlayer : MonoBehaviour
{
    public Transform player; // Arrastra aquí el Transform del jugador desde el inspector
    public float rotationSpeed = 5f; // Velocidad normal de rotación
    public float slowRotationSpeed = 2f; // Velocidad reducida para "esquivar"
    public float slowRotationDuration = 2f; // Tiempo que dura la rotación lenta
    public float slowRotationInterval = 5f; // Tiempo entre intervalos de rotación lenta

    private float nextSlowRotationTime = 0f;
    private float slowRotationEndTime = 0f;

    void Update()
    {
        if (player == null) return;

        // Calcular la dirección hacia el jugador
        Vector3 directionToPlayer = player.position - transform.position;
        directionToPlayer.y = 0; // Evitar que el jefe rote hacia arriba/abajo

        // Determinar la velocidad de rotación actual
        float currentRotationSpeed = rotationSpeed;

        if (Time.time >= nextSlowRotationTime)
        {
            currentRotationSpeed = slowRotationSpeed;
            if (slowRotationEndTime == 0f)
            {
                slowRotationEndTime = Time.time + slowRotationDuration;
            }

            // Si termina la rotación lenta, se programa el próximo intervalo
            if (Time.time >= slowRotationEndTime)
            {
                nextSlowRotationTime = Time.time + slowRotationInterval;
                slowRotationEndTime = 0f;
            }
        }

        // Rotar hacia el jugador
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, currentRotationSpeed * Time.deltaTime);
    }
}
