using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
    public float patrolWaitTime = 2f;
    public float attackDistance = 2f;
    public float moveSpeed = 2f;
    public float chaseSpeed = 4f;
    public int damage = 10;

    public int maxHealth = 100;
    private int currentHealth;

    private Transform player;
    private NavMeshAgent agent;
    private Animator animator;
    private bool isChasing;
    private bool isAttacking;
    private bool waiting;
    private int currentPatrolIndex;
    private Transform[] patrolPoints;
    private float detectionRadius;

    private bool isDead = false;

    public void Setup(EnemySpawner spawner, Transform[] patrolPoints, float detectionRadius)
    {
        this.patrolPoints = patrolPoints;
        this.detectionRadius = detectionRadius;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        GoToNextPatrolPoint();
    }

    void Update()
    {
        if (isDead) return;

        if (Vector3.Distance(transform.position, player.position) <= detectionRadius)
        {
            ChasePlayer();
        }
        else if (!isChasing && !waiting && !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            StartCoroutine(WaitAndPatrol());
        }

        if (!isAttacking)
        {
            UpdateAnimations();
        }

        if (isChasing && Vector3.Distance(transform.position, player.position) <= attackDistance && !isAttacking)
        {
            StartCoroutine(AttackPlayer());
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

        yield return new WaitForSeconds(1f); // Tiempo de enfriamiento del ataque

        isAttacking = false;
        agent.isStopped = false;
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            animator.SetTrigger("Hit"); // si tienes una animación de recibir golpe
        }
    }

    void Die()
    {
        isDead = true;
        agent.isStopped = true;
        animator.SetTrigger("Die"); // si tienes una animación de muerte
        Destroy(gameObject, 3f); // destruye al enemigo tras 3 segundos
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
        if (patrolPoints.Length == 0) return;
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

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ChasePlayer();
        }
    }
}
