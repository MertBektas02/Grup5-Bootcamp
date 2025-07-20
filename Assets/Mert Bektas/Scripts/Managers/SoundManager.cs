using System.Runtime.InteropServices;
using UnityEngine;
public enum SoundType
{
    DrinkWater,
    EatFood,
    CurrentDaySFX,
    ClickSound,
    ButtonHoverEffect,
    MachineWorking1,
    MachineWorking2,
    MachineWorking3,
    HitEffect,
    CowHurt1,
    CowHurt2,
    CowHurt3
}
[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] SoundList;
    private static SoundManager instance;
    private AudioSource audioSource;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public static void PlaySound(SoundType sound, float volume = 1f)
    {
        instance.audioSource.PlayOneShot(instance.SoundList[(int)sound]);
    }
    public static void PlayLoopSound(SoundType sound, float volume = 1f)
    {
        instance.audioSource.clip = instance.SoundList[(int)sound];
        instance.audioSource.volume = volume;
        instance.audioSource.loop = true;
        instance.audioSource.Play();
    }
    public static void StopLoopSound()
    {
        if (instance.audioSource.isPlaying && instance.audioSource.loop)
        {
            instance.audioSource.Stop();
            instance.audioSource.loop = false;
            instance.audioSource.clip = null;
        }
    }

    public static void PlayRandomSound(SoundType[] sounds, float volume = 1f)
    {
        if (sounds == null || sounds.Length == 0) return;

        int index = Random.Range(0, sounds.Length);
        AudioClip clip = instance.SoundList[(int)sounds[index]];
        instance.audioSource.PlayOneShot(clip, volume);
    }
}
