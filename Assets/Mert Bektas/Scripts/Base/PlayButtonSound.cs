using UnityEngine;

public class PlayButtonSound : MonoBehaviour
{
    public void PlayClickSound()
    {
        SoundManager.PlaySound(SoundType.ClickSound);
    }
}
