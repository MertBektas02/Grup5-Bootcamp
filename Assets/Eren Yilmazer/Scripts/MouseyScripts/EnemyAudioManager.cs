using UnityEngine;

public class EnemyAudioManager : MonoBehaviour
{
    public AudioSource audioSource;

    public AudioClip idleClip;
    public AudioClip walkClip;
    public AudioClip attackClip;
    public AudioClip deathClip;

    public void PlaySound(EnemySoundType type, bool loop = false)
    {
        AudioClip selectedClip = null;

        switch (type)
        {
            case EnemySoundType.Idle:
                selectedClip = idleClip;
                break;
            case EnemySoundType.Walk:
                selectedClip = walkClip;
                break;
            case EnemySoundType.Attack:
                selectedClip = attackClip;
                break;
            case EnemySoundType.Death:
                selectedClip = deathClip;
                break;
        }

        if (selectedClip != null)
        {
            audioSource.loop = loop;
            audioSource.clip = selectedClip;
            audioSource.Play();
        }
    }

    public void StopSound()
    {
        audioSource.Stop();
        audioSource.clip = null;
    }
}

public enum EnemySoundType
{
    Idle,
    Walk,
    Attack,
    Death
}