using System.Collections;
using UnityEngine;

public class IdleSound : StateMachineBehaviour
{
    public AudioClip idleMusic;
    public float maxDistance = 20f;
    public bool loop = true;

    private Transform player;
    private AudioSource source;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        source = animator.GetComponent<AudioSource>();

        if (source == null || player == null || idleMusic == null) return;

        source.clip = idleMusic;
        source.loop = loop;
        source.volume = 0f;
        source.Play();

        animator.GetComponent<MonoBehaviour>().StartCoroutine(UpdateVolumeWithDistance(animator.gameObject));
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (source != null)
        {
            source.Stop();
        }
    }

    private IEnumerator UpdateVolumeWithDistance(GameObject obj)
    {
        while (true)
        {
            if (player == null || source == null) yield break;

            float distance = Vector3.Distance(obj.transform.position, player.position);

            // Mesafeye göre volume ayarla (yakınsa 1.0, uzaktaysa 0.0)
            float normalized = Mathf.Clamp01(1f - (distance / maxDistance));
            source.volume = normalized;

            yield return new WaitForSeconds(0.1f);
        }
    }
}