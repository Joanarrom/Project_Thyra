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
    public bool dashEnabled = false;

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

    private void Start() //Obtiene la componente de CharacterController, y busca el Script de Player UI
    {
        characterController = GetComponent<CharacterController>();
        playerUI = FindObjectOfType<PlayerUI>(); 

        playerUI?.InitializeUI(maxHealth, health, maxEnergy, energy); 
    }

    private void Update() //Gestión de las acciones del Player
    {
        if (isParalyzed)
        {
            HandleParalysis();
            return;
        }

        // Solo permitir Dash si está habilitado
        if (dashEnabled && !isDashing && cooldownTimer <= 0f && energy >= dashEnergyCost && Input.GetKeyDown(KeyCode.LeftShift))
        {
            StartDash();
        }

        if (isDashing)
        {
            Dash();
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                FindNearestEnemy();
                isTargeting = target != null;
                Debug.Log(isTargeting ? "Fijado al enemigo más cercano." : "No se encontró ningún enemigo.");
            }

            if (isTargeting)
            {
                TargetedRotation();
            }

            MovePlayer();
        }

        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
        }
    }

    private void MovePlayer() //Controla el movimiento normal del jugador usando el CharacterController
                              
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

    private void StartDash() //Inicia el Dash
    {
        isDashing = true;
        dashTimer = dashDuration;
        cooldownTimer = dashCooldown;
        energy -= dashEnergyCost;

        playerUI?.UpdateEnergy(energy);

        if (energy <= 0) //Si la energia del Player llega a 0  se activa la paralisis
        {
            StartParalysis();
        }
    }

    private void Dash() //Ejeccución del moviemiento del Dash
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

    private Vector3 CalculateDashDirection() //Direccion del dash, basado en el movimiento del Player
    {
       
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        
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

    private void StartParalysis() //Activa la paralisis y configura su timer
    {
        isParalyzed = true;
        paralysisTimer = paralysisDuration;
    }

    private void HandleParalysis() //Reduce el tiempo del timer de la paralisis, al llegar a 0 se recupera una pequeña cantidad de energia
    {
        paralysisTimer -= Time.deltaTime;

        if (paralysisTimer <= 0f)
        {
            isParalyzed = false;
            energy += energyRecovery;
            playerUI?.UpdateEnergy(energy);
        }
    }

    public void TakeDamage(int damage) //Aplicar el daño al jugador
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

        // Actualizar el PlayerUI
        if (FindObjectOfType<PlayerUI>() != null)
        {
            FindObjectOfType<PlayerUI>().UpdateHealth(health);
        }
    }

    private void Die() // Cargar escena GameOver
    { 
    
        SceneManager.LoadScene("GameOver"); 
    
    }

    public void RestoreHealth(int percentage) //Restaura la vida en funcion de su % de vida maxima (Cambiar esta funcion para mas adelante a un % fijo para incrementar la dificultad)
    {
        int restoreAmount = maxHealth * percentage / 100;
        health = Mathf.Clamp(health + restoreAmount, 0, maxHealth);
        playerUI?.UpdateHealth(health);
    }

    public void RestoreEnergy(int percentage) //Misma funcion que el de la vida (Aplicar el mismo cambio)
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
            if (col.CompareTag("Enemy")) // Identifica al Objeto mas cercano con el Tag "Enemy"
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

    // Rotación hacia el enemigo fijado y calcula la direccion hacia el enemigo
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