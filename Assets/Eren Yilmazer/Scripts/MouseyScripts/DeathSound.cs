using UnityEngine;
using System.Collections;

public class DeathSound : StateMachineBehaviour
{
    public AudioClip deathClip;
    public float delay = 1f;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!deathClip) return;

        CoroutineRunner runner = animator.GetComponent<CoroutineRunner>();
        if (runner)
        {
            runner.StartCoroutine(PlayAttackSoundDelayed(animator.GetComponent<AudioSource>()));
        }
    }

    private IEnumerator PlayAttackSoundDelayed(AudioSource source)
    {
        yield return new WaitForSeconds(delay);

        if (source && deathClip)
        {
            source.PlayOneShot(deathClip);
        }
    }
}
