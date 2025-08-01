using UnityEngine;

public class MovementSound : MonoBehaviour
{
    public AudioSource movementAudioSource;
    public AudioClip moveSound;
    public float minSpeed = 0.1f;
    public float soundInterval = 0.4f;

    private Vector3 lastPosition;
    private float startDelay = 0.2f; // ilk karede ses �almas�n� �nlemek i�in

    void Start()
    {
        if (movementAudioSource == null)
        {
            Debug.LogError("Ad�m sesi AudioSource atanmad�!");
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
        // Oyun ba�layal� �ok k�sa s�re olduysa hi�bir �ey yapma
        if (Time.timeSinceLevelLoad < startDelay)
            return;

        Vector3 movement = transform.position - lastPosition;
        float speed = movement.magnitude / Time.deltaTime;

        if (speed > minSpeed)
        {
            if (!movementAudioSource.isPlaying)
            {
                movementAudioSource.Play();
                Debug.Log("Ad�m sesi BA�LADI");
            }
        }
        else
        {
            if (movementAudioSource.isPlaying)
            {
                movementAudioSource.Stop();
                Debug.Log("Ad�m sesi DURDU");
            }
        }

        lastPosition = transform.position;
    }
}
