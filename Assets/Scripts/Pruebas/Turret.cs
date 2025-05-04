using System.Collections; 
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Turret : MonoBehaviour
{
    [Header("Attack Settings")]
    public GameObject bulletPrefab;  // Prefab del proyectil
    public Transform firePoint;      // Punto de origen del disparo (cañón)
    public float bulletSpeed = 15f;  // Velocidad del proyectil
    public float detectionRange = 10f; // Rango de detección del jugador
    public float fireRate = 2f;  // Tiempo entre disparos
    public float rotationSpeed = 5f;  // Velocidad de rotación hacia el jugador

    [Header("Animation Settings")]
    public string shootingClipName = "Shoot";  // Nombre del clip de disparo
    public string idleClipName = "Idle";  // Nombre del clip de inactividad
    [Range(0, 0.3f)] public float animationTransitionTime = 0.1f;  // Tiempo de transición entre animaciones

    private Transform player;  // El jugador al que la torreta debe apuntar
    private float fireCooldown;  // Tiempo restante antes de disparar de nuevo
    private bool isShooting;  // Indicador de si la torreta está disparando
    private Animator animator;  // El Animator de la torreta
    private int shootingAnimHash;  // Hash de la animación de disparo
    private int idleAnimHash;  // Hash de la animación idle
    private float shootingAnimLength;  // Duración de la animación de disparo

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;  // Encuentra al jugador
        animator = GetComponent<Animator>();  // Obtén el Animator
        shootingAnimHash = Animator.StringToHash(shootingClipName);
        idleAnimHash = Animator.StringToHash(idleClipName);
        shootingAnimLength = GetAnimationLength(shootingClipName);
        ValidateSettings();  // Verifica las configuraciones iniciales
    }

    // Obtiene la duración de una animación específica
    float GetAnimationLength(string clipName)
    {
        if (animator == null) return 0;
        foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips)
        {
            if (clip.name == clipName) return clip.length;
        }
        return 0;
    }

    // Verifica que los componentes estén correctamente configurados
    void ValidateSettings()
    {
        if (!animator) { Debug.LogError("Animator no encontrado!", gameObject); enabled = false; }
        if (shootingAnimLength <= 0) Debug.LogError($"Clip '{shootingClipName}' no encontrado!", gameObject);
        if (GetAnimationLength(idleClipName) <= 0) Debug.LogError($"Clip '{idleClipName}' no encontrado!", gameObject);
    }

    void Update()
    {
        if (!player || !animator) return;  // Si no hay jugador o animator, no hace nada

        float distance = Vector3.Distance(transform.position, player.position);  // Calcula la distancia al jugador

        if (distance <= detectionRange)  // Si está dentro del rango de detección
        {
            RotateTowardsPlayer();  // Rota hacia el jugador
            HandleShootingLogic();  // Controla la lógica de disparo
        }
        else
        {
            PlayIdleAnimation();  // Si no está cerca, juega la animación de inactividad
        }

        fireCooldown -= Time.deltaTime;  // Disminuye el tiempo de recarga de disparo
    }

    // Rota el cuerpo de la torreta (pivot) hacia el jugador
    void RotateTowardsPlayer()
    {
        // Aquí calculamos la dirección hacia el jugador, pero solo sobre el plano horizontal (ignora Y)
        Vector3 direction = player.position - transform.position;  
        direction.y = 0;  // Asegúrate de que no se gire hacia arriba o hacia abajo

        // Calculamos la rotación deseada, pero invertimos la dirección de la rotación
        Quaternion targetRotation = Quaternion.LookRotation(direction);  // Dirección hacia el jugador

        // Invertimos la rotación en el eje Y para que apunte correctamente con el cañón
        targetRotation = Quaternion.Euler(0, targetRotation.eulerAngles.y + 180, 0); 

        // Rota el cuerpo de la torreta (pivot) hacia el jugador
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);  // Rota solo el cuerpo de la torreta, no el cañón
    }

    // Controla la lógica de disparo
    void HandleShootingLogic()
    {
        if (!isShooting && fireCooldown <= 0)
        {
            StartCoroutine(ShootingRoutine());  // Inicia la rutina de disparo
        }
    }

    // Rutina para disparar
    IEnumerator ShootingRoutine()
    {
        isShooting = true;  // Marca que está disparando
        animator.SetBool("isShooting", true);  // Activa la animación de disparo
        fireCooldown = fireRate;  // Reinicia el tiempo de recarga

        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Shoot"));  // Espera a que termine la animación de disparo

        Fire();  // Dispara el proyectil

        isShooting = false;  // Marca que ha terminado de disparar
        animator.SetBool("isShooting", false);  // Desactiva la animación de disparo
    }

    // Reproduce la animación de inactividad si no está disparando
    void PlayIdleAnimation()
    {
        if (!isShooting)
        {
            animator.CrossFade(idleAnimHash, animationTransitionTime);  // Transición suave a la animación idle
        }
    }

    // Lógica para disparar el proyectil
    void Fire()
    {
        if (!firePoint || !bulletPrefab) return;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);  // Instancia el proyectil

        Vector3 direction = (player.position - firePoint.position).normalized;  // Calcula la dirección del disparo
        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        if (rb)
        {
            rb.velocity = direction * bulletSpeed;  // Aplica velocidad al proyectil
        }
        else
        {
            Debug.LogWarning("El proyectil no tiene Rigidbody!", bullet);
        }
    }

    // Dibuja un Gizmo para mostrar el rango de detección en el editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);  // Dibuja un círculo amarillo para mostrar el rango
    }
}