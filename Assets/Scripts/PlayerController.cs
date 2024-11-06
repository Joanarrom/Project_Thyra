using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;            // Velocidad de movimiento del jugador
    public float rotationSpeed = 10f;       // Velocidad de rotación del jugador
    public Transform cameraTransform;       // La cámara que sigue al jugador
    
    public float dashSpeed = 15f;           // Velocidad del dash
    public float dashDuration = 0.2f;       // Duración del dash en segundos
    public float dashCooldown = 1f;         // Tiempo de espera entre dashes

    private CharacterController characterController;
    private bool isDashing = false;         // Estado de dash
    private float dashTimer = 0f;           // Temporizador para la duración del dash
    private float cooldownTimer = 0f;       // Temporizador para el cooldown

    void Start()
    {
        // Obtener el componente CharacterController en el jugador
        characterController = GetComponent<CharacterController>();

        // Comprobamos si la cámara está asignada
        if (cameraTransform == null)
        {
            Debug.LogWarning("No se ha asignado una cámara al controlador de jugador.");
        }
    }

    void Update()
    {
        // Si no estamos en cooldown de dash, podemos verificar el dash
        if (!isDashing && cooldownTimer <= 0f && Input.GetKeyDown(KeyCode.LeftShift))
        {
            StartDash();
        }

        // Si estamos en modo dash, actualizamos el dash
        if (isDashing)
        {
            Dash();
        }
        else
        {
            // Si no estamos en dash, ejecutamos el movimiento normal del jugador
            MovePlayer();
        }

        // Reducir el cooldown del dash en función del tiempo
        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
        }
    }

    void MovePlayer()
    {
        // Obtener la entrada en los ejes Horizontal (A/D) y Vertical (W/S)
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Crear el vector de dirección con base en la entrada y la orientación de la cámara
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        // Si hay alguna entrada de movimiento, procedemos
        if (direction.magnitude >= 0.1f)
        {
            // Obtener el ángulo en el que debe rotar el personaje en función de la cámara
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;

            // Crear una rotación en esa dirección
            Quaternion rotation = Quaternion.Euler(0f, targetAngle, 0f);

            // Rotar suavemente hacia el ángulo objetivo
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);

            // Calcular el vector de movimiento final en la dirección del jugador
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            // Mover al jugador usando CharacterController
            characterController.Move(moveDir.normalized * moveSpeed * Time.deltaTime);
        }
    }

    void StartDash()
    {
        isDashing = true;
        dashTimer = dashDuration;
        cooldownTimer = dashCooldown;
    }

    void Dash()
    {
        // Calcular la dirección hacia adelante (forward) en función de la rotación actual del personaje
        Vector3 dashDirection = transform.forward;

        // Mover al personaje en la dirección del dash a la velocidad del dash
        characterController.Move(dashDirection * dashSpeed * Time.deltaTime);

        // Reducir el tiempo del dash en función del tiempo
        dashTimer -= Time.deltaTime;

        // Si el tiempo de dash ha llegado a cero, finalizar el dash
        if (dashTimer <= 0f)
        {
            isDashing = false;
        }
    }
}
