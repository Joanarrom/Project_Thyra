using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
    [Header("Patrullaje y detección")]
    public float patrolWaitTime = 2f;
    public float detectionRadius = 10f;
    public Transform[] patrolPoints;

    [Header("Movimiento")]
    public float moveSpeed = 2f;
    public float chaseSpeed = 4f;

    [Header("Ataque")]
    public float attackDistance = 2f;
    public int damage = 10;
    public float attackCooldown = 2f;
    private float lastAttackTime;

    [Header("Vida")]
    public int maxHealth = 100;
    private int currentHealth;

    private Transform player;
    private NavMeshAgent agent;
    private Animator animator;

    private bool isChasing;
    private bool isAttacking;
    private bool waiting;
    private int currentPatrolIndex;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void Setup(EnemySpawner spawner, Transform[] patrolPoints, float detectionRadius)
    {
        this.patrolPoints = patrolPoints;
        this.detectionRadius = detectionRadius;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        GoToNextPatrolPoint();
    }

    void Update()
    {
        if (player == null || agent == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRadius)
        {
            if (distanceToPlayer <= attackDistance && !isAttacking && Time.time - lastAttackTime >= attackCooldown)
            {
                StartCoroutine(AttackPlayer());
            }
            else
            {
                ChasePlayer();
            }
        }
        else if (!isChasing && !waiting && !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            StartCoroutine(WaitAndPatrol());
        }

        if (!isAttacking)
        {
            UpdateAnimations();
        }
    }

    IEnumerator AttackPlayer()
    {
        isAttacking = true;
        animator.SetTrigger("Attack");
        agent.isStopped = true;

        ThirdPersonController playerController = player.GetComponent<ThirdPersonController>();
        if (playerController != null)
        {
            playerController.TakeDamage(damage);
        }

        lastAttackTime = Time.time;

        yield return new WaitForSeconds(1f); // Duración de la animación de ataque
        isAttacking = false;
        agent.isStopped = false;

        if (Vector3.Distance(transform.position, player.position) > attackDistance)
        {
            GoToNextPatrolPoint();
        }
    }

    IEnumerator WaitAndPatrol()
    {
        waiting = true;
        agent.isStopped = true;
        animator.SetFloat("Speed", 0f);
        yield return new WaitForSeconds(patrolWaitTime);
        GoToNextPatrolPoint();
        waiting = false;
        agent.isStopped = false;
    }

    void GoToNextPatrolPoint()
    {
        if (patrolPoints == null || patrolPoints.Length == 0) return;

        currentPatrolIndex = Random.Range(0, patrolPoints.Length);
        agent.speed = moveSpeed;
        agent.SetDestination(patrolPoints[currentPatrolIndex].position);
        isChasing = false;
    }

    void UpdateAnimations()
    {
        float speed = agent.velocity.magnitude;
        animator.SetFloat("Speed", speed > 0.1f ? moveSpeed : 0f);
    }

    void ChasePlayer()
    {
        isChasing = true;
        agent.speed = chaseSpeed;
        agent.SetDestination(player.position);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Llamar al evento de muerte y dar energía al jugador de inmediato
        animator.SetTrigger("Die");
        agent.isStopped = true;
        GetComponent<Collider>().enabled = false;
        this.enabled = false;

        // Otorgar energía al jugador si está cerca
        ThirdPersonController playerController = player.GetComponent<ThirdPersonController>();
        if (playerController != null)
        {
            playerController.OnEnemyKilled();
        }

      EnemyScore score = GetComponent<EnemyScore>();
        if (score != null)
        {
         score.Morir(); // Esto sumará puntos y destruirá el objeto
        }
     else
     {
         Destroy(gameObject); // Fallback por si no tiene EnemyScore
     }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ChasePlayer();
        }
    }
}