using UnityEngine;
using UnityEngine.AI;

public class AnimalMovement : MonoBehaviour
{
    public float wanderRadius = 5f;
    public float waitTime = 2f;

    public AudioClip animalSound;
    public float minSoundDelay = 5f;
    public float maxSoundDelay = 15f;

    public float hearingDistance = 12f; // 🔊 Oyuncuya olan mesafe limiti
    public Transform player; // 🧍 Oyuncu referansı

    private NavMeshAgent agent;
    private float waitTimer;
    private Animator animator;
    private AudioSource audioSource;

    private float soundTimer;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        ChooseNewDestination();
        waitTimer = waitTime;

        agent.updateRotation = false;
        agent.updateUpAxis = false;

        soundTimer = Random.Range(minSoundDelay, maxSoundDelay);
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

        soundTimer -= Time.deltaTime;
        if (soundTimer <= 0f)
        {
            if (player != null && Vector3.Distance(transform.position, player.position) <= hearingDistance)
            {
                PlayAnimalSound();
            }
            soundTimer = Random.Range(minSoundDelay, maxSoundDelay);
        }
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
            if (sprite)
            {
                sprite.flipX = dir.x > 0.1f;
            }
        }
    }

    void PlayAnimalSound()
    {
        if (animalSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(animalSound);
        }
    }
}
