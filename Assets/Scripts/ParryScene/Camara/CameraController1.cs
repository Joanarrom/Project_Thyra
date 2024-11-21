using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController1 : MonoBehaviour


{
    public Transform target;               
    public float distance = 5.0f;         
    public float horizontalSensitivity = 100f; 
    public float verticalSensitivity = 100f;   
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
        // Rotación horizontal 
        yaw += Input.GetAxis("Mouse X") * horizontalSensitivity * Time.deltaTime;

        // Rotación vertical 
        pitch += Input.GetAxis("Mouse Y") * verticalSensitivity * Time.deltaTime;

        // Limitar el ángulo vertical 
        pitch = Mathf.Clamp(pitch, -30f, 60f);
    }

    void LateUpdate()
    {
        
        Vector3 direction = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        
        
        transform.position = target.position + Vector3.up * verticalOffset + rotation * direction;
       
        transform.LookAt(target.position + Vector3.up * verticalOffset);
    }
}


