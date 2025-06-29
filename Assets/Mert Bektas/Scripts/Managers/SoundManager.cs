using System.Runtime.InteropServices;
using UnityEngine;
public enum SoundType
{
    DrinkWater,
    EatFood
}
[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] SoundList;
   private static SoundManager instance;
   private AudioSource audioSource;

    private void Awake()
    {
        instance=this;
    }
    private void Start()
    {
        audioSource=GetComponent<AudioSource>();
    }

    public static void PlaySound(SoundType sound, float volume=1f)
   {
        instance.audioSource.PlayOneShot(instance.SoundList[(int)sound]);
   }
}
