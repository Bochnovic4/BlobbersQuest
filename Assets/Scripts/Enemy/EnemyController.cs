using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public Transform player;
    public float viewDistance = 10f;
    public float attackDistance = 1f;
    public float attackInterval = 30f; 

    private NavMeshAgent agent;
    private Animator enemyAnimator;
    private float lastAttackTime = 0f;
    private bool isAttacking = false;
    [SerializeField]
    private EnemyWeapon weapon;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        enemyAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (isAttacking)
        {
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= viewDistance)
        {
            ChasePlayer();
        }
        else
        {
            StopChase();
        }

        UpdateAnimationState();
    }

    private void ChasePlayer()
    {
        agent.isStopped = false;
        agent.SetDestination(player.position);

        if (Vector3.Distance(transform.position, player.position) <= attackDistance)
        {
            AttackPlayer();
        }
    }

    private void StopChase()
    {
        agent.isStopped = true;
        enemyAnimator.SetBool("isWalking", false);
    }

    private void AttackPlayer()
    {
        if (Time.time - lastAttackTime >= attackInterval)
        {
            isAttacking = true;
            agent.isStopped = true;
            enemyAnimator.SetBool("isWalking", false);
            enemyAnimator.SetTrigger("attack");
            weapon.EnableCollider();

            lastAttackTime = Time.deltaTime;


            Invoke(nameof(EndAttack), 2f);
        }
    }

    private void EndAttack()
    {
        isAttacking = false;
        agent.isStopped = false;
        weapon.DisableCollider();
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
    }
}
