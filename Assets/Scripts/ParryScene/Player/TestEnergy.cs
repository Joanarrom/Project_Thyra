using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnergy : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;
    public Transform cameraTransform;
    public float dashSpeed = 15f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;

    public int maxHealth = 100; // Vida máxima
    public int maxEnergy = 100; // Energía máxima
    public int health = 100;
    public int energy = 100; // Energía actual
    public int dashEnergyCost = 25; // Energía necesaria para hacer dash
    public int energyRecovery = 20; // Energía recuperada tras parálisis
    public float paralysisDuration = 2f; // Tiempo de parálisis al agotar energía

    private CharacterController characterController;
    private bool isDashing = false;
    private bool isParalyzed = false;
    private float dashTimer = 0f;
    private float cooldownTimer = 0f;
    private float paralysisTimer = 0f;
    private Vector3 velocity; // Velocidad del jugador, incluyendo gravedad

    // Referencia al script de UI
   public PlayerUI playerUI;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerUI = FindObjectOfType<PlayerUI>(); // Busca el script de UI en la escena

        playerUI?.InitializeUI(maxHealth, health, maxEnergy, energy); // Inicializa las barras
    }

   private void Update()
    {
        if (isParalyzed)
        {
            HandleParalysis();
            return;
        }

        if (!isDashing && cooldownTimer <= 0f && energy >= dashEnergyCost && Input.GetKeyDown(KeyCode.LeftShift))
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
            velocity.y += Physics.gravity.y * Time.deltaTime;
        }
        else
        {
            velocity.y = 0f;
        }

        characterController.Move(velocity * Time.deltaTime);
    }

    private void StartDash()
    {
        isDashing = true;
        dashTimer = dashDuration;
        cooldownTimer = dashCooldown;
        energy -= dashEnergyCost;

        playerUI?.UpdateEnergy(energy);

        if (energy <= 0)
        {
            StartParalysis();
        }
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

    private void StartParalysis()
    {
        isParalyzed = true;
        paralysisTimer = paralysisDuration;
    }

    private void HandleParalysis()
    {
        paralysisTimer -= Time.deltaTime;

        if (paralysisTimer <= 0f)
        {
            isParalyzed = false;
            energy += energyRecovery;
            playerUI?.UpdateEnergy(energy);
        }
    }

    public void TakeDamage(int damage)
    {
         if (damage <= 0)
    {
        Debug.LogWarning("El daño debe ser un valor positivo.");
        return;
    }

    if (isDashing)
    {
        Debug.Log("El jugador está en dash, no recibe daño.");
        return;
    }

    health -= damage;
    Debug.Log("El jugador recibió " + damage + " puntos de daño. Salud actual: " + health);

    if (health <= 0)
    {
        health = 0; // Aseguramos que la salud no sea negativa
        Die();
    }

    // Actualizar la UI
    if (FindObjectOfType<PlayerUI>() != null)
    {
        FindObjectOfType<PlayerUI>().UpdateHealth(health);
    }
    }

    private void Die()
    {
        Destroy(gameObject);
        Debug.Log("El jugador ha muerto.");
    }

    public void RestoreHealth(int percentage)
    {
        int restoreAmount = maxHealth * percentage / 100;
        health = Mathf.Clamp(health + restoreAmount, 0, maxHealth);
        playerUI?.UpdateHealth(health);
    }

    public void RestoreEnergy(int percentage)
    {
        int restoreAmount = maxEnergy * percentage / 100;
        energy = Mathf.Clamp(energy + restoreAmount, 0, maxEnergy);
        playerUI?.UpdateEnergy(energy);
    }
}

