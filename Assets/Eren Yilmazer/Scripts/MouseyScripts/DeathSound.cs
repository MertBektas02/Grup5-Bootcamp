using UnityEngine;
using System.Collections;

public class DeathSound : StateMachineBehaviour
{
    public AudioClip deathClip;
    public float delay = 1f;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (deathClip == null) return;

        CoroutineRunner runner = animator.GetComponent<CoroutineRunner>();
        if (runner != null)
        {
            runner.StartCoroutine(PlayAttackSoundDelayed(animator.GetComponent<AudioSource>()));
        }
    }

    private IEnumerator PlayAttackSoundDelayed(AudioSource source)
    {
        yield return new WaitForSeconds(delay);

        if (source != null && deathClip != null)
        {
            source.PlayOneShot(deathClip);
        }
    }
}
