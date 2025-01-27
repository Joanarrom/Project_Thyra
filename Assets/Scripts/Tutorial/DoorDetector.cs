using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorDetector : MonoBehaviour
{
   public Transform door; // Referencia al transform de la puerta que se va a mover
    public Vector3 openPositionOffset; // Offset relativo a la posición inicial para cuando la puerta está abierta
    public float speed = 2f; // Velocidad de deslizamiento de la puerta
    public float delayToClose = 1f; // Tiempo de espera antes de cerrar

    private Vector3 closedPosition; // Posición inicial de la puerta
    private Vector3 openPosition; // Posición final de la puerta
    private Coroutine doorCoroutine; // Corrutina activa para mover la puerta
    private bool isPlayerDetected = false; // Verifica si el jugador está en el trigger

    private void Start()
    {
        if (door == null)
        {
            Debug.LogError("Debes asignar el Transform de la puerta en el inspector.");
            return;
        }

        // Define las posiciones de la puerta
        closedPosition = door.position;
        openPosition = closedPosition + openPositionOffset;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Detecta si el jugador entra en el trigger
        {
            isPlayerDetected = true;
            if (doorCoroutine != null) StopCoroutine(doorCoroutine); // Detiene cualquier corrutina activa
            doorCoroutine = StartCoroutine(MoveDoor(openPosition)); // Abre la puerta
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) // Detecta si el jugador sale del trigger
        {
            isPlayerDetected = false;
            StartCoroutine(CloseDoorWithDelay()); // Espera un tiempo antes de cerrar la puerta
        }
    }

    private IEnumerator CloseDoorWithDelay()
    {
        yield return new WaitForSeconds(delayToClose); // Espera antes de cerrar la puerta
        if (!isPlayerDetected) // Asegúrate de que el jugador no esté en el trigger
        {
            if (doorCoroutine != null) StopCoroutine(doorCoroutine); // Detiene cualquier corrutina activa
            doorCoroutine = StartCoroutine(MoveDoor(closedPosition)); // Cierra la puerta
        }
    }

    private IEnumerator MoveDoor(Vector3 targetPosition)
    {
        while (Vector3.Distance(door.position, targetPosition) > 0.01f)
        {
            door.position = Vector3.Lerp(door.position, targetPosition, Time.deltaTime * speed); // Mueve la puerta suavemente
            yield return null;
        }
        door.position = targetPosition; // Asegura que la puerta termine exactamente en su posición objetivo
    }
}
