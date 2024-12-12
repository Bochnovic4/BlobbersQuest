using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;
public class EnemyController : MonoBehaviour
{
    public Transform player; // Assign the player GameObject in the inspector
    public float viewDistance = 10f; // Distance at which the enemy notices the player
    public float attackDistance = 1f; // Distance at which the enemy starts attacking
    public float attackInterval = 30f; // Time between attacks
    public float patrolRadius = 15f; // Radius within which to find patrol points
    public float patrolCooldown = 5f; // Time between patrol movements

    private NavMeshAgent agent;
    private Animator enemyAnimator;
    private float lastAttackTime = 0f;
    private float lastPatrolTime = 0f;
    private float patrolCooldownTimer = 0f;
    private Vector3 patrolTarget;
    private bool isAttacking = false;
    private bool isPatrolling = false;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        enemyAnimator = GetComponent<Animator>();
        StartPatrol();
    }

    private void Update()
    {

        if (isAttacking)
        {
            return; // Prevent interruptions during attack
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= viewDistance)
        {
            ChasePlayer();
        }
        else
        {
            PatrolBehavior();
        }

        UpdateAnimationState();
    }

    private void ChasePlayer()
    {
        isPatrolling = false;
        agent.isStopped = false;
        agent.SetDestination(player.position);

        if (Vector3.Distance(transform.position, player.position) <= attackDistance)
        {
            AttackPlayer();
        }
    }

    private void AttackPlayer()
    {
        if (Time.time - lastAttackTime >= attackInterval)
        {
            isAttacking = true;
            agent.isStopped = true; // Stop movement while attacking
            enemyAnimator.SetBool("isWalking", false);
            enemyAnimator.SetTrigger("attack");

            lastAttackTime = Time.time;

            // Resume movement after attack animation
            Invoke(nameof(EndAttack), 2f); // Match this time with the attack animation length
        }
    }

    private void EndAttack()
    {
        isAttacking = false;
        agent.isStopped = false; // Resume movement
    }

    private void PatrolBehavior()
    {
        if (isPatrolling)
        {
            if (!agent.pathPending && agent.remainingDistance < 0.5f)
            {
                isPatrolling = false; // Patrol point reached
                patrolCooldownTimer = patrolCooldown; // Reset cooldown
            }
        }
        else
        {
            patrolCooldownTimer -= Time.deltaTime;

            if (patrolCooldownTimer <= 0f)
            {
                StartPatrol();
            }
        }
    }

    private void StartPatrol()
    {
        isPatrolling = true;

        Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;
        randomDirection += transform.position;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, patrolRadius, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
            agent.isStopped = false;
        }
        else
        {
            Debug.Log("Failed to find a valid patrol point. Retrying...");
            Invoke(nameof(StartPatrol), 1f); // Retry after delay
        }
    }

    private void UpdateAnimationState()
    {
        if (agent.velocity.sqrMagnitude > 0.1f && !isAttacking)
        {
            enemyAnimator.SetBool("isWalking", true);
        }
        else if (!isAttacking)
        {
            enemyAnimator.SetBool("isWalking", false);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, viewDistance);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackDistance);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, patrolRadius);
    }


}
