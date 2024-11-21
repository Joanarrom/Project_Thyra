using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;
    public Transform cameraTransform;
    public float dashSpeed = 15f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;
    public int health = 100;

    public HealthBar healthBar;

    private CharacterController characterController;
    private bool isDashing = false;
    private float dashTimer = 0f;
    private float cooldownTimer = 0f;
    private Vector3 velocity; // Velocidad del jugador, incluyendo gravedad

    private void Start()
    {
        characterController = GetComponent<CharacterController>();

        if (cameraTransform == null)
        {
            Debug.LogWarning("No se ha asignado una cámara al controlador de jugador.");
        }

        if (healthBar != null)
        {
            healthBar.SetHealth(health);
        }
    }

    private void Update()
    {
        if (!isDashing && cooldownTimer <= 0f && Input.GetKeyDown(KeyCode.LeftShift))
        {
            StartDash();
        }

        if (isDashing)
        {
            Dash();
        }
        else
        {
            MovePlayer();
        }

        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
        }
    }

    private void MovePlayer()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
            Quaternion rotation = Quaternion.Euler(0f, targetAngle, 0f);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            characterController.Move(moveDir * moveSpeed * Time.deltaTime);
        }

        // Aplicar gravedad
        if (!characterController.isGrounded)
        {
            velocity.y += Physics.gravity.y * Time.deltaTime; // Gravedad acumulativa
        }
        else
        {
            velocity.y = 0f; // Restablecer la velocidad vertical al tocar el suelo
        }

        // Aplicar la velocidad vertical al CharacterController
        characterController.Move(velocity * Time.deltaTime);
    }

    private void StartDash()
    {
        isDashing = true;
        dashTimer = dashDuration;
        cooldownTimer = dashCooldown;
    }

    private void Dash()
    {
        Vector3 dashDirection = transform.forward;
        characterController.Move(dashDirection * dashSpeed * Time.deltaTime);
        dashTimer -= Time.deltaTime;

        if (dashTimer <= 0f)
        {
            isDashing = false;
        }
    }

    // Método para reducir la vida del jugador
    public void TakeDamage(int damage)
    {
        if (isDashing)
        {
            Debug.Log("El jugador está en dash, no recibe daño.");
            return; // Salir sin aplicar daño si el jugador está en dash
        }

        health -= damage;
        Debug.Log("Salud del jugador: " + health);

        if (healthBar != null)
        {
            healthBar.SetHealth(health);
        }

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(this.gameObject);  
        Debug.Log("¡El jugador ha muerto!");
        // Implementar lógica de muerte aquí
    }
}