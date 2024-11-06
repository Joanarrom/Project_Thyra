using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController1 : MonoBehaviour


{
    public Transform target;               // Objetivo a seguir (el jugador)
    public float distance = 5.0f;          // Distancia de la cámara al jugador
    public float horizontalSensitivity = 100f; // Sensibilidad para rotación horizontal
    public float verticalSensitivity = 100f;   // Sensibilidad para rotación vertical
    public float verticalOffset = 1.5f;    // Altura de la cámara respecto al jugador

    private float pitch = 0.0f;            // Ángulo vertical
    private float yaw = 0.0f;              // Ángulo horizontal

    void Start()
    {
        // Ocultar y bloquear el cursor en el centro de la pantalla
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Rotación horizontal usando horizontalSensitivity
        yaw += Input.GetAxis("Mouse X") * horizontalSensitivity * Time.deltaTime;

        // Rotación vertical usando verticalSensitivity e invertida
        pitch += Input.GetAxis("Mouse Y") * verticalSensitivity * Time.deltaTime;

        // Limitar el ángulo vertical para evitar que la cámara dé un giro completo
        pitch = Mathf.Clamp(pitch, -30f, 60f);
    }

    void LateUpdate()
    {
        // Calcular la posición deseada de la cámara con base en la rotación y distancia
        Vector3 direction = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        
        // Posición final de la cámara con el offset vertical y la rotación aplicada
        transform.position = target.position + Vector3.up * verticalOffset + rotation * direction;
        
        // Mirar hacia el objetivo (jugador)
        transform.LookAt(target.position + Vector3.up * verticalOffset);
    }
}


