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
    public int health = 100;                // Vida del jugador

    public HealthBar healthBar;             // Referencia a la barra de vida

    private CharacterController characterController;
    private bool isDashing = false;
    private float dashTimer = 0f;
    private float cooldownTimer = 0f;

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        if (cameraTransform == null)
        {
            Debug.LogWarning("No se ha asignado una cámara al controlador de jugador.");
        }

        // Inicializar la barra de vida con la salud actual del jugador
        if (healthBar != null)
        {
            healthBar.SetHealth(health);
        }
    }

    void Update()
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

    void MovePlayer()
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

    // Actualizar la barra de vida
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
        Debug.Log("¡El jugador ha muerto!");
        // Aquí puedes implementar lógica para el fin del juego o reiniciar el nivel
    }
}
