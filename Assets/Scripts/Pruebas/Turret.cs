using System.Collections; 
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Turret : MonoBehaviour
{
    [Header("Attack Settings")]
    [Tooltip("Prefab del proyectil a disparar")]
    public GameObject bulletPrefab;
    
    [Tooltip("Punto de origen del disparo")]
    public Transform firePoint;
    
    [Tooltip("Velocidad del proyectil")]
    public float bulletSpeed = 15f;
    
    [Tooltip("Rango de detección del jugador")]
    public float detectionRange = 10f;
    
    [Tooltip("Tiempo entre disparos")]
    public float fireRate = 2f;
    
    [Tooltip("Velocidad de rotación hacia el jugador")]
    public float rotationSpeed = 5f;

    [Header("Animation Settings")]
    [Tooltip("Nombre EXACTO del Animation Clip de disparo")]
    public string shootingClipName = "Shoot";
    
    [Tooltip("Nombre EXACTO del Animation Clip de idle")]
    public string idleClipName = "Idle";
    
    [Tooltip("Tiempo de transición entre animaciones")]
    [Range(0, 0.3f)]
    public float animationTransitionTime = 0.1f;

    // Variables privadas
    private Transform player;
    private float fireCooldown;
    private bool isShooting;
    private Animator animator;
    private int shootingAnimHash;
    private int idleAnimHash;
    private float shootingAnimLength;

    void Start()
    {
        InitializeComponents();
        CacheAnimations();
        ValidateSettings();
    }

    void InitializeComponents()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        animator = GetComponent<Animator>();
    }

    void CacheAnimations()
    {
        shootingAnimHash = Animator.StringToHash(shootingClipName);
        idleAnimHash = Animator.StringToHash(idleClipName);
        shootingAnimLength = GetAnimationLength(shootingClipName);
    }

    float GetAnimationLength(string clipName)
    {
        if (animator == null) return 0;

        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            if (clip.name == clipName)
                return clip.length;
        }
        return 0;
    }

    void ValidateSettings()
    {
        if (!animator)
        {
            Debug.LogError("Animator no encontrado!", gameObject);
            enabled = false;
            return;
        }

        if (shootingAnimLength <= 0)
            Debug.LogError($"Clip de disparo '{shootingClipName}' no encontrado!", gameObject);

        if (GetAnimationLength(idleClipName) <= 0)
            Debug.LogError($"Clip de idle '{idleClipName}' no encontrado!", gameObject);
    }

    void Update()
    {
        if (!player || !animator) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= detectionRange)
        {
            RotateTowardsPlayer();
            HandleShootingLogic();
        }
        else
        {
            PlayIdleAnimation();
        }
    }

    void RotateTowardsPlayer()
    {
        Vector3 direction = player.position - transform.position;
        direction.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(
            transform.rotation, 
            targetRotation, 
            rotationSpeed * Time.deltaTime
        );
    }

    void HandleShootingLogic()
    {
        if (!isShooting && fireCooldown <= 0)
        {
            StartCoroutine(ShootingRoutine());
        }
    }

    IEnumerator ShootingRoutine()
    {
        isShooting = true; // Cambiar a true para reproducir la animación "Shoot"
        animator.SetBool("isShooting", true); // Cambiar el valor de la variable en el Animator

        fireCooldown = fireRate;

        // Iniciar animación de disparo (esto ahora está controlado por el Animator)
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Shoot"));

        // Ejecutar disparo
        Fire();

        // Volver a idle
        isShooting = false; // Cambiar a false para volver a la animación "Idle"
        animator.SetBool("isShooting", false); // Cambiar el valor de la variable en el Animator
    }

    void PlayIdleAnimation()
    {
        if (!isShooting)
        {
            animator.CrossFade(idleAnimHash, animationTransitionTime);
        }
    }

    void Fire()
    {
        if (!firePoint || !bulletPrefab) return;

        GameObject bullet = Instantiate(
            bulletPrefab, 
            firePoint.position, 
            firePoint.rotation
        );

        Vector3 direction = (player.position - firePoint.position).normalized;
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        
        if (rb)
        {
            rb.velocity = direction * bulletSpeed;
        }
        else
        {
            Debug.LogWarning("El proyectil no tiene Rigidbody!", bullet);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}