using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
    public float patrolWaitTime = 2f;
    public float attackDistance = 2f;
    public float moveSpeed = 2f;
    public float chaseSpeed = 4f;

    private EnemySpawner spawner;
    private Transform[] patrolPoints;
    private Transform player;
    private NavMeshAgent agent;
    private Animator animator;
    private float detectionRadius;
    private bool isChasing;
    private int currentPatrolIndex;
    private bool waiting;

    public void Setup(EnemySpawner spawner, Transform[] patrolPoints, float detectionRadius)
    {
        this.spawner = spawner;
        this.patrolPoints = patrolPoints;
        this.detectionRadius = detectionRadius;
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        GoToNextPatrolPoint();
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, player.position) <= detectionRadius)
        {
            ChasePlayer();
        }
        else if (!isChasing && !waiting && !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            StartCoroutine(WaitAndPatrol());
        }

        UpdateAnimations();
    }

    void ChasePlayer()
    {
        isChasing = true;
        agent.speed = chaseSpeed;
        agent.SetDestination(player.position);

        if (Vector3.Distance(transform.position, player.position) <= attackDistance)
        {
            animator.SetTrigger("Attack");
            agent.isStopped = true;
        }
        else
        {
            agent.isStopped = false;
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
        if (patrolPoints.Length == 0) return;
        currentPatrolIndex = Random.Range(0, patrolPoints.Length);
        agent.speed = moveSpeed;
        agent.SetDestination(patrolPoints[currentPatrolIndex].position);
        isChasing = false;
    }

    void UpdateAnimations()
    {
        float speed = agent.velocity.magnitude;

        if (isChasing)
            animator.SetFloat("Speed", speed > 0.1f ? chaseSpeed : 0f);
        else
            animator.SetFloat("Speed", speed > 0.1f ? moveSpeed : 0f);
    }
}
