using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PruebaMovimientoAI : MonoBehaviour
{public Transform player;               
    public float detectionRange = 15f;     
    public float followRange = 10f;        
    public float stopDistance = 2f;        
    public float attackRange = 3f;        
    public float attackCooldown = 1f;      
    public int damageAmount = 20;         
    public float fieldOfView = 60f;       

    private NavMeshAgent agent;
    private float lastAttackTime;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        lastAttackTime = -attackCooldown;
    }

    void Update()
    {
        
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        
        if (distanceToPlayer <= detectionRange && IsPlayerInSight())
        {
            if (distanceToPlayer > stopDistance && distanceToPlayer <= followRange)
            {
               
                agent.SetDestination(player.position);
            }
            else if (distanceToPlayer <= stopDistance)
            {
                
                agent.ResetPath();
            }

            
            if (distanceToPlayer <= attackRange)
            {
                AttackPlayer();
            }
        }
        else
        {
            
            agent.ResetPath();
        }
    }

    
    bool IsPlayerInSight()
    {
        
        Vector3 directionToPlayer = (player.position - transform.position).normalized;

      
        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

       
        if (angleToPlayer < fieldOfView / 2f)
        {
           
            if (Physics.Raycast(transform.position, directionToPlayer, out RaycastHit hit, detectionRange))
            {
                
                if (hit.transform == player)
                {
                    return true; 
                }
            }
        }

        return false; 
    }

    void AttackPlayer()
    {
       
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            PlayerController playerController = player.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.TakeDamage(damageAmount);
                Debug.Log("El enemigo ataca al jugador infligiendo " + damageAmount + " de daño.");
            }

            lastAttackTime = Time.time;
        }
    }
}
