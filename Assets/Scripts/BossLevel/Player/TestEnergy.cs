using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestEnergy : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;
    public Transform cameraTransform;
    public float dashSpeed = 15f;
    public float dashDuration = 1f;
    public float dashCooldown = 1f;

    public int maxHealth = 300; // Vida máxima
    public int maxEnergy = 100; // Energía máxima
    public int health = 300;
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

    // Variables para el sistema de fijación a enemigos
    public float targetingRange = 50f; // Rango para encontrar enemigos
    private Transform target; // El objetivo al que nos vamos a fijar
    private bool isTargeting = false; // Si el jugador está fijado al objetivo

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
            if (Input.GetKeyDown(KeyCode.Q)) // Fijarse al enemigo más cercano cuando se presiona "Q"
            {
                FindNearestEnemy();
                isTargeting = target != null;
                if (isTargeting)
                {
                    Debug.Log("Fijado al enemigo más cercano.");
                }
                else
                {
                    Debug.Log("No se encontró ningún enemigo.");
                }
            }

            if (isTargeting)
            {
                TargetedRotation(); // Rotación hacia el enemigo fijado
            }

            MovePlayer(); // Movimiento normal del jugador (moviéndose lateralmente, pero girando hacia el objetivo si está fijado)
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
            // Movimiento normal
            Vector3 moveDir = cameraTransform.TransformDirection(direction);
            moveDir.y = 0f; // Asegurarse de que no se mueva hacia arriba o hacia abajo
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
        // Cálculo de la dirección del dash
        Vector3 dashDirection = CalculateDashDirection();
        characterController.Move(dashDirection * dashSpeed * Time.deltaTime);
        dashTimer -= Time.deltaTime;

        if (dashTimer <= 0f)
        {
            isDashing = false;
        }
    }

    private Vector3 CalculateDashDirection()
    {
        // Obtener la dirección del movimiento (horizontal y vertical)
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Inicializar la dirección de movimiento
        Vector3 dashDirection = Vector3.zero;

        // Si el jugador se está moviendo hacia adelante o hacia atrás
        if (vertical > 0) // Hacia adelante
        {
            dashDirection = transform.forward;
        }
        else if (vertical < 0) // Hacia atrás
        {
            dashDirection = -transform.forward;
        }
        // Si el jugador se mueve lateralmente (izquierda o derecha)
        else if (horizontal != 0)
        {
            dashDirection = transform.right * horizontal;
        }

        dashDirection.y = 0f; // Asegurarse de que el dash sea solo horizontal

        return dashDirection;
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
    
        SceneManager.LoadScene("GameOver"); // Regresa al menú principal
    
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

    // Buscar al enemigo más cercano
    private void FindNearestEnemy()
    {
        Collider[] enemiesInRange = Physics.OverlapSphere(transform.position, targetingRange);
        Transform closestEnemy = null;
        float minDistance = Mathf.Infinity;

        foreach (Collider col in enemiesInRange)
        {
            if (col.CompareTag("Enemy")) // Asumiendo que los enemigos están etiquetados como "Enemy"
            {
                float distance = Vector3.Distance(transform.position, col.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestEnemy = col.transform;
                }
            }
        }

        target = closestEnemy;
    }

    // Rotación hacia el enemigo fijado
    private void TargetedRotation()
    {
        if (target != null)
        {
            Vector3 targetDirection = target.position - transform.position;
            targetDirection.y = 0; // Ignorar la variación en la altura

            if (targetDirection.magnitude >= 0.1f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
    }
}