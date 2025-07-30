using UnityEngine;

public class WalkSound : StateMachineBehaviour
{
    public AudioClip walkClip;
    private AudioSource audioSource;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!audioSource)
            audioSource = animator.GetComponent<AudioSource>();

        if (audioSource && walkClip)
        {
            audioSource.clip = walkClip;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (audioSource && audioSource.clip == walkClip)
        {
            audioSource.Stop();
        }
    }
}