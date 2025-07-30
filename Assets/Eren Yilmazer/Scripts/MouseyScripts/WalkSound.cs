using UnityEngine;

public class WalkSound : StateMachineBehaviour
{
    public AudioClip walkClip;
    private AudioSource _audioSource;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!_audioSource)
            _audioSource = animator.GetComponent<AudioSource>();

        if (_audioSource && walkClip)
        {
            _audioSource.clip = walkClip;
            _audioSource.loop = true;
            _audioSource.Play();
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_audioSource && _audioSource.clip == walkClip)
        {
            _audioSource.Stop();
        }
    }
}