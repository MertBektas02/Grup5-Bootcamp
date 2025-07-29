using UnityEngine;

public class MovementSound : MonoBehaviour
{
    public AudioSource movementAudioSource;
    public AudioClip moveSound;
    public float minSpeed = 0.1f;
    public float soundInterval = 0.4f;

    private Vector3 lastPosition;
    private float startDelay = 0.2f; // ilk karede ses çalmasýný önlemek için
    private float timer = 0f;

    void Start()
    {
        if (movementAudioSource == null)
        {
            Debug.LogError("Adým sesi AudioSource atanmadý!");
            enabled = false;
            return;
        }

        movementAudioSource.clip = moveSound;
        movementAudioSource.loop = true;
        movementAudioSource.playOnAwake = false;

        lastPosition = transform.position;
    }

    void Update()
    {
        // Oyun baþlayalý çok kýsa süre olduysa hiçbir þey yapma
        if (Time.timeSinceLevelLoad < startDelay)
            return;

        Vector3 movement = transform.position - lastPosition;
        float speed = movement.magnitude / Time.deltaTime;

        if (speed > minSpeed)
        {
            if (!movementAudioSource.isPlaying)
            {
                movementAudioSource.Play();
                Debug.Log("Adým sesi BAÞLADI");
            }
        }
        else
        {
            if (movementAudioSource.isPlaying)
            {
                movementAudioSource.Stop();
                Debug.Log("Adým sesi DURDU");
            }
        }

        lastPosition = transform.position;
    }
}
