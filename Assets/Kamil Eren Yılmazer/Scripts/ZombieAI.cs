using UnityEngine;
using UnityEngine.AI;

public class ZombieAI : MonoBehaviour
{
    public Transform player;
    public float attackRange = 2f;
    public float chaseRange = 15f;
    public int health = 100;
    public LayerMask visionObstructionMask;

    public float wanderRadius = 10f;    // Dolaşacağı alan yarıçapı
    public float wanderWaitTime = 3f;   // Her hedefte bekleme süresi

    private NavMeshAgent agent;
    private Animator animator;
    private bool isDead = false;

    private float idleTimer = 0f;
    private Vector3 wanderTarget;
    private bool isWandering = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        ChooseNewWanderTarget();
        idleTimer = wanderWaitTime;
    }

    void Update()
    {
        if (isDead || player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= chaseRange && HasLineOfSight())
        {
            // Oyuncuyu görürse takip veya saldırı moduna geç
            isWandering = false;

            if (distanceToPlayer <= attackRange)
            {
                agent.isStopped = true;
                animator.SetBool("isWalking", false);
                animator.SetBool("isAttacking", true);
            }
            else
            {
                agent.isStopped = false;
                agent.SetDestination(player.position);
                animator.SetBool("isWalking", true);
                animator.SetBool("isAttacking", false);
            }
        }
        else
        {
            // Oyuncuyu görmüyorsa dolaşma moduna geç
            animator.SetBool("isAttacking", false);

            if (!isWandering)
            {
                ChooseNewWanderTarget();
                isWandering = true;
                idleTimer = wanderWaitTime;
            }

            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                idleTimer -= Time.deltaTime;
                animator.SetBool("isWalking", false);

                if (idleTimer <= 0f)
                {
                    ChooseNewWanderTarget();
                    agent.SetDestination(wanderTarget);
                    animator.SetBool("isWalking", true);
                    idleTimer = wanderWaitTime;
                }
            }
            else
            {
                animator.SetBool("isWalking", true);
            }

            agent.isStopped = false;
        }
    }

    void ChooseNewWanderTarget()
    {
        Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
        randomDirection += transform.position;

        if (NavMesh.SamplePosition(randomDirection, out NavMeshHit navHit, wanderRadius, NavMesh.AllAreas))
        {
            wanderTarget = navHit.position;
            agent.SetDestination(wanderTarget);
        }
        else
        {
            wanderTarget = transform.position; // Başarısızsa yerinde kal
        }
    }

    bool HasLineOfSight()
    {
        Vector3 origin = transform.position + Vector3.up * 1.5f;
        Vector3 target = player.position + Vector3.up * 1.5f;
        Vector3 direction = (target - origin).normalized;
        float distance = Vector3.Distance(origin, target);

        if (Physics.Raycast(origin, direction, out RaycastHit hit, distance, visionObstructionMask))
        {
            return hit.collider.gameObject == player.gameObject;
        }

        return true;
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;

        health -= amount;
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        agent.isStopped = true;
        animator.SetBool("isDead", true);
        animator.SetBool("isWalking", false);
        animator.SetBool("isAttacking", false);
        Destroy(gameObject, 5f);
    }
}
