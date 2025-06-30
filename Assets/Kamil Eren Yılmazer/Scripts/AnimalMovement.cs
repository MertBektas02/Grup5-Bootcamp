using UnityEngine;
using UnityEngine.AI;

public class AnimalMovement : MonoBehaviour
{
    public float wanderRadius = 5f;
    public float waitTime = 2f;

    private NavMeshAgent agent;
    private float waitTimer;
    

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        
        ChooseNewDestination();
        waitTimer = waitTime;
        // 2D sprite kullanımı için zorunlu ayarlar
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        
    }

    void Update()
    {
        if (!agent.isOnNavMesh) return;

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
        if (dir.x < -0.1f)
            transform.rotation = Quaternion.Euler(0f, 180f, 0f); // sola bak
        else if (dir.x > 0.1f)
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);   // sağa bak
    }
}