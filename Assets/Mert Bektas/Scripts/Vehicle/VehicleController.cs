using UnityEngine;

public class VehicleController : MonoBehaviour
{
    [Header("Araç Ayarları")]
    public float acceleration = 15f;
    public float maxSpeed = 20f;
    public float turnSpeed = 60f;

    private Rigidbody rb;
    private bool isActive = false;
    private bool isGrounded;

    [Header("Audio")]
    public AudioClip accelClip;  // sabitten hızlanırken
    public AudioClip loopClip;   // max hızda sürekli
    public AudioClip decelClip;  // hız düşerken

    private AudioSource sfxSource;   // tek seferlik sesler için
    private AudioSource loopSource;  // motor loop için

    private enum VehicleAudioState { Idle, Accelerating, Cruising, Decelerating }
    private VehicleAudioState currentAudioState = VehicleAudioState.Idle;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        // Tek seferlik sesler için
        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.loop = false;
        sfxSource.playOnAwake = false;
        sfxSource.spatialBlend = 1f; // 3D ses

        // Loop için ayrı bir AudioSource
        loopSource = gameObject.AddComponent<AudioSource>();
        loopSource.clip = loopClip;
        loopSource.loop = true;
        loopSource.playOnAwake = false;
        loopSource.spatialBlend = 1f; // 3D ses
    }

    void FixedUpdate()
    {
        if (!isActive) return;

        CheckGround();
        if (!isGrounded) return;

        HandleMovement();
        HandleTurning();
        ReduceSidewaysSlip();

        UpdateAudioState();
    }

    private void HandleMovement()
    {
        float moveInput = Input.GetAxis("Vertical");
        Vector3 currentVelocity = rb.linearVelocity;
        Vector3 localVel = transform.InverseTransformDirection(currentVelocity);

        if (moveInput != 0 && Mathf.Abs(localVel.z) < maxSpeed)
        {
            Vector3 forwardVelocity = transform.forward * moveInput * acceleration;
            rb.AddForce(forwardVelocity, ForceMode.Acceleration);
        }
    }

    private void HandleTurning()
    {
        float turnInput = Input.GetAxis("Horizontal");
        Vector3 localVel = transform.InverseTransformDirection(rb.linearVelocity);

        if (Mathf.Abs(localVel.z) > 0.1f)
        {
            transform.Rotate(Vector3.up, turnInput * turnSpeed * Time.fixedDeltaTime);
        }
    }

    private void ReduceSidewaysSlip()
    {
        Vector3 sideways = transform.right * Vector3.Dot(rb.linearVelocity, transform.right);
        rb.AddForce(-sideways * 5f, ForceMode.Acceleration);
    }

    private void CheckGround()
    {
        isGrounded = Physics.Raycast(
            transform.position + Vector3.up * 0.5f,
            Vector3.down,
            out RaycastHit hit,
            1f
        );
    }

    // Ses durumlarını hız değerine göre güncelle
    private void UpdateAudioState()
    {
        float speed = rb.linearVelocity.magnitude;

        float accelThreshold = 1f;   // sabitten çıkarken
        float cruiseThreshold = 8f;  // max hız hissi

        if (speed > accelThreshold && speed < cruiseThreshold)
        {
            SetAudioState(VehicleAudioState.Accelerating);
        }
        else if (speed >= cruiseThreshold)
        {
            SetAudioState(VehicleAudioState.Cruising);
        }
        else if (speed < accelThreshold && currentAudioState != VehicleAudioState.Idle)
        {
            SetAudioState(VehicleAudioState.Decelerating);
        }
    }

    private void SetAudioState(VehicleAudioState newState)
    {
        if (newState == currentAudioState) return;

        switch (newState)
        {
            case VehicleAudioState.Accelerating:
                sfxSource.PlayOneShot(accelClip);
                loopSource.Stop();
                break;

            case VehicleAudioState.Cruising:
                if (!loopSource.isPlaying) loopSource.Play();
                break;

            case VehicleAudioState.Decelerating:
                sfxSource.PlayOneShot(decelClip);
                loopSource.Stop();
                break;

            case VehicleAudioState.Idle:
                loopSource.Stop();
                break;
        }

        currentAudioState = newState;
    }

    public void SetActive(bool state)
    {
        isActive = state;
        if (!state)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            SetAudioState(VehicleAudioState.Idle);
        }
    }

    public bool IsStopped()
    {
        return rb.linearVelocity.magnitude < 0.1f;
    }
}
