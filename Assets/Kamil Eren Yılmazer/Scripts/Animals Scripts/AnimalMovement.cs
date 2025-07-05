using UnityEngine;
using UnityEngine.AI;

public class AnimalMovement : MonoBehaviour
{
    public float wanderRadius = 5f;
    public float waitTime = 2f;

    private NavMeshAgent agent;
    private float waitTimer;
    
    private Animator animator;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        
        ChooseNewDestination();
        waitTimer = waitTime;
        // 2D sprite kullanımı için zorunlu ayarlar
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        
    }

    void Update()
    {
        if (!agent.isOnNavMesh) return;
        bool isWalking = agent.velocity.magnitude > 0.1f;
        animator.SetBool("isWalking", isWalking);

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            waitTimer -= Time.deltaTime;

            if (waitTimer <= 0f)
            {
                ChooseNewDestination();
                waitTimer = waitTime;
            }
        }
        
            RotateTowardsDirection();
    }

    void ChooseNewDestination()
    {
        Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
        randomDirection.y = 0f;
        Vector3 target = transform.position + randomDirection;

        if (NavMesh.SamplePosition(target, out NavMeshHit hit, wanderRadius, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
            
        }
       
    }

    void RotateTowardsDirection()
    {
        Vector3 dir = agent.velocity;
        if (dir.magnitude > 0.1f)
        {
            SpriteRenderer sprite = GetComponent<SpriteRenderer>();
            if (sprite != null)
            {
                if (dir.x < -0.1f)
                    sprite.flipX = false; // sola bak
                else if (dir.x > 0.1f)
                    sprite.flipX = true; // sağa bak
            }
        }
    }

}