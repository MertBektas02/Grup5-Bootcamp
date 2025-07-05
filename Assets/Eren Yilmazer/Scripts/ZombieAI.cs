using UnityEngine;
using UnityEngine.AI;

public class ZombieAI : MonoBehaviour
{
    public Transform player;
    public float attackRange = 2f;
    public float chaseRange = 15f;
    public int health = 100;
    

    public float wanderRadius = 10f;    // Dolaşacağı alan yarıçapı
    public float wanderWaitTime = 3f;   // Her hedefte bekleme süresi

    private NavMeshAgent agent;
    private Animator animator;
    public bool isDead = false;

    private float idleTimer = 0f;
    private Vector3 wanderTarget;
    private bool isWandering = false;
    
    private float attackCooldown = 1.5f;
    private float attackTimer = 0f;
    
    private bool isBlinded = false;
    private float blindTimer = 0f;

    public GameObject activeFlashBomb; // Flash bombası sahnedeyse atanacak
    

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        ChooseNewWanderTarget(); // Zombinin ilk wander hedefini belirle
        idleTimer = wanderWaitTime;
        isWandering = true;
        Debug.DrawLine(transform.position, wanderTarget, Color.yellow, 2f);
    }

    void Update()
    {
        
        if (isDead || player == null) return;
        // FLASH MODU: Flash bombası aktifse ona yürü
        if (activeFlashBomb != null && activeFlashBomb.activeSelf&& activeFlashBomb.scene.IsValid())
        {
            agent.isStopped = false;
            animator.SetBool("isWalking", true);
            animator.SetBool("isAttacking", false);
            agent.SetDestination(activeFlashBomb.transform.position);
            return;
        }
        
        
        if (isBlinded)
        {
            blindTimer -= Time.deltaTime;

            // Flash bombaya yürümeye devam etsin
            animator.SetBool("isAttacking", false);

            if (blindTimer <= 0f)
            {
                isBlinded = false;
                
            }

            return;
        }
        

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= chaseRange && HasLineOfSight())
        {
            
            isWandering = false;

            if (distanceToPlayer <= attackRange)
            {
                agent.isStopped = true;
                animator.SetBool("isWalking", false);
                animator.SetBool("isAttacking", true);
                attackTimer -= Time.deltaTime;
                if (attackTimer <= 0f)
                {
                    TryDamagePlayer();
                    attackTimer = attackCooldown;
                }
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
    void TryDamagePlayer()
    {
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(10f); // 10 birim hasar ver
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

        if (Physics.Raycast(origin, direction, out RaycastHit hit, distance))
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
        
        float deathAnimLength = GetAnimationClipLength("Zombie Death"); // animasyon ismiyle birebir eşleşmeli
        Destroy(gameObject, deathAnimLength);

    }
    float GetAnimationClipLength(string clipName)
    {
        foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips)
        {
            if (clip.name == clipName)
                return clip.length;
        }
        return 5f; // fallback
    }
    public void BecomeBlinded(float duration, Vector3 distractionPosition)
    {
        if (isBlinded || isDead || agent == null) return;

        isBlinded = true;
        blindTimer = duration;

        agent.isStopped = false;
        agent.SetDestination(distractionPosition);

        animator.SetBool("isWalking", true);
        
    }
  

}
