using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
public class ThirdPersonController : MonoBehaviour
{
    [Header("Referencias Base")]
    public Camera playerCamera;
    public Transform cameraPivot;
    public Slider energySlider;
    public Animator animator;
    public GameObject weaponCollider;

    [Header("Movimiento")]
    public float walkSpeed = 5f;
    public float sprintSpeed = 8f;
    public float rotationSpeed = 10f;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public float animationSmoothTime = 0.1f;

    [Header("Energía")]
    public float maxEnergy = 100f;
    public float energyDepletionRate = 4f;
    public float energyPerKill = 250f;
    public float criticalEnergyThreshold = 20f;
    public Color criticalEnergyColor = Color.red;

    [Header("Esquiva")]
    public float dodgeSpeed = 12f;
    public float dodgeDuration = 0.6f;
    public float dodgeEnergyCost = 25f;
    public float dodgeCooldown = 1.2f;
    public KeyCode dodgeKey = KeyCode.Space;

    [Header("Gravedad")]
    public float gravity = -30f;
    public float groundCheckRadius = 0.4f;
    public Transform groundCheck;
    public LayerMask groundMask;

    [Header("Combate")]
    public int lightAttackDamage = 25;
    public float lightAttackEnergy = 15f;
    public float lightAttackCooldown = 1f;
    public int heavyAttackDamage = 40;
    public float heavyAttackEnergy = 30f;
    public float heavyAttackCooldown = 2.5f;
    public float knockbackForce = 8f;

    // Animator Hashes
    private int animIDSpeed;
    private int animIDGrounded;
    private int animIDLightAttack;
    private int animIDHeavyAttack;
    private int animIDDamage;
    private int animIDDie;
    private int animIDDodge;

    // Variables de estado
    private CharacterController controller;
    private float currentEnergy;
    private bool isDead;
    private float verticalVelocity;
    private bool isGrounded;
    private bool isDodging;
    private float dodgeTimer;
    private Vector3 dodgeDirection;
    private bool hasInvincibility;
    private float nextLightAttack;
    private float nextHeavyAttack;
    private bool isAttacking;
    private List<GameObject> hitTargets = new List<GameObject>();

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        currentEnergy = maxEnergy;
        UpdateEnergyUI();
        
        if(playerCamera == null)
            playerCamera = Camera.main;

        AssignAnimationIDs();
       
    }

    void AssignAnimationIDs()
    {
        animIDSpeed = Animator.StringToHash("Speed");
        animIDGrounded = Animator.StringToHash("IsGrounded");
        animIDLightAttack = Animator.StringToHash("LightAttack");
        animIDHeavyAttack = Animator.StringToHash("HeavyAttack");
        animIDDamage = Animator.StringToHash("TakeDamage");
        animIDDie = Animator.StringToHash("Die");
        animIDDodge = Animator.StringToHash("Dodge");
    }

    void Update()
    {
        if(isDead) return;
        
        HandleGravity();
        HandleDodge();
        
        if(!isDodging && !isAttacking)
        {
            HandleMovement();
            HandleCamera();
            HandleCombat();
        }
        
        UpdateEnergy();
        UpdateAnimations();
    }

    void UpdateAnimations()
    {
        float currentSpeed = controller.velocity.magnitude / sprintSpeed;
        animator.SetFloat(animIDSpeed, currentSpeed, animationSmoothTime, Time.deltaTime);
        animator.SetBool(animIDGrounded, isGrounded);
    }

    void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 cameraForward = Vector3.Scale(playerCamera.transform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 moveDirection = (vertical * cameraForward + horizontal * playerCamera.transform.right).normalized;

        float speedMultiplier = Input.GetKey(sprintKey) && currentEnergy > 0 ? sprintSpeed : walkSpeed;

        if(moveDirection != Vector3.zero)
        {
            controller.Move(moveDirection * speedMultiplier * Time.deltaTime);
            
            Quaternion targetRot = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRot,
                rotationSpeed * Time.deltaTime
            );
        }
    }

    void HandleCamera()
    {
        float mouseX = Input.GetAxis("Mouse X") * 100f * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * 100f * Time.deltaTime;

        cameraPivot.Rotate(Vector3.up * mouseX);
        cameraPivot.Rotate(Vector3.left * mouseY);
        
        Vector3 angles = cameraPivot.localEulerAngles;
        angles.x = Mathf.Clamp(angles.x > 180 ? angles.x - 360 : angles.x, -30, 60);
        cameraPivot.localEulerAngles = angles;
    }

    void HandleCombat()
    {
        if(Time.time >= nextLightAttack && Input.GetMouseButtonDown(0) && currentEnergy >= lightAttackEnergy)
        {
            StartCoroutine(PerformAttack(true));
            nextLightAttack = Time.time + lightAttackCooldown;
        }

        if(Time.time >= nextHeavyAttack && Input.GetMouseButtonDown(1) && currentEnergy >= heavyAttackEnergy)
        {
            StartCoroutine(PerformAttack(false));
            nextHeavyAttack = Time.time + heavyAttackCooldown;
        }
    }

    IEnumerator PerformAttack(bool isLightAttack)
    {
       isAttacking = true;
        hitTargets.Clear();
    
        // Corrección: Usar int para los hashes de animación
     int triggerHash = isLightAttack ? animIDLightAttack : animIDHeavyAttack;
        int damage = isLightAttack ? lightAttackDamage : heavyAttackDamage;
        float energyCost = isLightAttack ? lightAttackEnergy : heavyAttackEnergy;
    
        animator.SetTrigger(triggerHash); // ← Usar el hash directamente
        currentEnergy -= energyCost;
        UpdateEnergyUI();

       
    
        yield return new WaitForSeconds(isLightAttack ? 0.3f : 0.5f);
    
     
    isAttacking = false;
    }

    public void OnWeaponHit(Collider other)
    {
        if(!isAttacking || hitTargets.Contains(other.gameObject)) return;

     hitTargets.Add(other.gameObject);

     if(other.TryGetComponent<EnemyAI>(out EnemyAI enemyAI))
        {
          enemyAI.TakeDamage(lightAttackDamage);
        }
     else if(other.TryGetComponent<PowerSource>(out PowerSource powerSource))
        {
            powerSource.TakeDamage(lightAttackDamage);
        }
    }

    void HandleDodge()
    {
        dodgeTimer -= Time.deltaTime;

        if(Input.GetKeyDown(dodgeKey) && CanDodge())
        {
            StartCoroutine(PerformDodge());
        }
    }

    bool CanDodge()
    {
        return dodgeTimer <= 0 && 
               currentEnergy >= dodgeEnergyCost && 
               !isDodging && 
               isGrounded;
    }

    IEnumerator PerformDodge()
    {
        isDodging = true;
        hasInvincibility = true;
        currentEnergy -= dodgeEnergyCost;
        dodgeTimer = dodgeCooldown;
        animator.SetTrigger(animIDDodge);
        UpdateEnergyUI();

        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        if(input == Vector3.zero) input = -transform.forward;

        dodgeDirection = playerCamera.transform.TransformDirection(input).normalized;
        dodgeDirection.y = 0;

        float timer = 0;
        while(timer < dodgeDuration)
        {
            controller.Move(dodgeDirection * dodgeSpeed * Time.deltaTime);
            timer += Time.deltaTime;
            yield return null;
        }

        isDodging = false;
        hasInvincibility = false;
    }

    void HandleGravity()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundMask, QueryTriggerInteraction.Ignore);

        if(isGrounded && verticalVelocity < 0)
            verticalVelocity = -2f;

        verticalVelocity += gravity * Time.deltaTime;
        controller.Move(Vector3.up * verticalVelocity * Time.deltaTime);
    }

    void UpdateEnergy()
    {
        currentEnergy -= energyDepletionRate * Time.deltaTime;
        currentEnergy = Mathf.Clamp(currentEnergy, 0f, maxEnergy);
        UpdateEnergyUI();

        if(currentEnergy <= 0f)
            Die();
    }

    public void OnEnemyKilled()
    {
        currentEnergy += energyPerKill;
        currentEnergy = Mathf.Clamp(currentEnergy, 0f, maxEnergy);
        UpdateEnergyUI();
    }

    void UpdateEnergyUI()
    {
        if(energySlider != null)
        {
            energySlider.value = currentEnergy / maxEnergy;
            Image fillImage = energySlider.fillRect.GetComponent<Image>();
            fillImage.color = currentEnergy <= criticalEnergyThreshold ? criticalEnergyColor : Color.green;
        }
    }

    public void TakeDamage(int damage)
    {
        if(!hasInvincibility && !isDead)
        {
            animator.SetTrigger(animIDDamage);
            currentEnergy -= damage;
            UpdateEnergyUI();
        }
    }

    void Die()
    {
        isDead = true;
        animator.SetTrigger(animIDDie);
        controller.enabled = false;
        enabled = false;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }

    public void EnableInvincibility() => hasInvincibility = true;
    public void DisableInvincibility() => hasInvincibility = false;
}