using System.Collections;
using UnityEngine;

public class AmbientSoundManager : MonoBehaviour
{
    [Header("Audio Sources")]
    public AudioSource windSound1;
    public AudioSource windSound2;
    public AudioSource birdSound3;
    public AudioSource birdSound2;
    public AudioSource birdSound4;

    [Header("Audio Clips")]
    public AudioClip[] windSounds;
    public AudioClip[] windSounds2;
    public AudioClip[] birdSounds3;
    public AudioClip[] birdSounds2;
    public AudioClip[] birdSounds4;

    [Header("Background Music")]
    public AudioSource bgmSource;
    public AudioClip bgmClip;

    void Start()
    {
        windSound1.gameObject.SetActive(true);
        windSound2.gameObject.SetActive(true);
        birdSound3.gameObject.SetActive(true);
        birdSound2.gameObject.SetActive(true);
        birdSound4.gameObject.SetActive(true);

        StartCoroutine(PlayRandomSoundLoop(windSound1, windSounds, 1f, 7f));
        StartCoroutine(PlayRandomSoundLoop(windSound2, windSounds2, 7f, 9f));
        StartCoroutine(PlayRandomSoundLoop(birdSound3, birdSounds3, 8f, 12f));
        StartCoroutine(PlayRandomSoundLoop(birdSound2, birdSounds2, 7f, 10f));
        StartCoroutine(PlayRandomSoundLoop(birdSound4, birdSounds4, 3f, 7f));

        if (bgmSource != null && bgmClip != null)
        {
            bgmSource.clip = bgmClip;
            bgmSource.loop = true;
            bgmSource.Play();
        }
    }

    IEnumerator PlayRandomSoundLoop(AudioSource source, AudioClip[] clips, float minDelay, float maxDelay)
    {
        if (source == null || clips == null || clips.Length == 0)
        {
            Debug.LogWarning("AudioSource veya AudioClip dizisi eksik!");
            yield break;
        }

        while (true)
        {
            float waitTime = Random.Range(minDelay, maxDelay);
            yield return new WaitForSeconds(waitTime);

            if (!source.enabled)
            {
                Debug.LogWarning($"{source.name} AudioSource kapalýydý, yeniden açýldý.");
                source.enabled = true;
            }

            if (!source.gameObject.activeInHierarchy)
            {
                Debug.LogWarning($"{source.name} GameObject inactive, ses çalýnamadý.");
                continue;
            }

            AudioClip clip = clips[Random.Range(0, clips.Length)];
            source.PlayOneShot(clip);
        }
    }
}
