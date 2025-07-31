using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Animator))]
public class EnemySoundManager : MonoBehaviour
{
    [Header("Sound Clips")]
    public AudioClip idleSound;
    public AudioClip walkSound;
    public AudioClip attackSound;
    public AudioClip deathSound;
    public AudioClip playerHurtSound;

    [Header("Sound Settings")]
    [Range(0, 1)] public float volume = 0.5f;
    public float idleSoundInterval = 8f;
    public float walkSoundInterval = 0.5f;

    [Header("3D Sound Settings")]
    public float minDistance = 2f;
    public float maxDistance = 10f;

    private AudioSource audioSource;
    private Animator animator;
    private float nextIdleTime;
    private float nextWalkTime; // EKSİK OLAN TANIM EKLENDİ
    private bool isPlayingIdleSound;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();

        // AudioSource ayarları
        audioSource.spatialBlend = 1f;
        audioSource.rolloffMode = AudioRolloffMode.Linear;
        audioSource.minDistance = minDistance;
        audioSource.maxDistance = maxDistance;
        audioSource.volume = volume;

        nextIdleTime = Time.time + idleSoundInterval;
        nextWalkTime = Time.time + walkSoundInterval; // BAŞLANGIÇ DEĞERİ ATANDI
    }

    void Update()
    {
        if (animator.GetBool("isDead")) return;

        CheckIdleState();
        CheckWalkState();
    }

    private void CheckIdleState()
    {
        bool isInIdleState = animator.GetCurrentAnimatorStateInfo(0).IsName("MouseyIdle");

        if (isInIdleState)
        {
            if (Time.time >= nextIdleTime && !isPlayingIdleSound)
            {
                PlayIdleSound();
                nextIdleTime = Time.time + idleSoundInterval;
            }
        }
        else if (isPlayingIdleSound)
        {
            audioSource.Stop();
            isPlayingIdleSound = false;
        }
    }

    private void CheckWalkState()
    {
        if (animator.GetBool("isWalking"))
        {
            if (Time.time >= nextWalkTime)
            {
                PlayWalkSound();
                nextWalkTime = Time.time + walkSoundInterval;
            }
        }
    }

    private void PlayIdleSound()
    {
        if (idleSound != null)
        {
            audioSource.clip = idleSound;
            audioSource.loop = false;
            audioSource.Play();
            isPlayingIdleSound = true;
        }
    }

    private void PlayWalkSound()
    {
        if (walkSound != null && !audioSource.isPlaying)
        {
            audioSource.PlayOneShot(walkSound);
        }
    }

    public void PlayAttackSound()
    {
        if (attackSound != null)
            audioSource.PlayOneShot(attackSound);
    }

    public void PlayDeathSound()
    {
        if (deathSound != null)
            audioSource.PlayOneShot(deathSound);
    }

    public void PlayPlayerHurtSound()
    {
        if (playerHurtSound != null)
            audioSource.PlayOneShot(playerHurtSound);
    }
}