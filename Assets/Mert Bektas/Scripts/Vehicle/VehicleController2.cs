using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class VehicleController2 : MonoBehaviour
{
    [Header("Araç Ayarları")]
    public float acceleration = 15f;
    public float maxSpeed = 20f;
    public float turnSpeed = 60f;

    private Rigidbody rb;
    private bool isActive = false;
    private bool isGrounded;

    [Header("Motor Sesi")]
    public AudioClip loopClip;     // Tek bir motor sesi
    public float minPitch = 0.8f;  // yavaşken pitch
    public float maxPitch = 1.8f;  // hızlıyken pitch
    public float minVolume = 0.2f; // yavaşken volume
    public float maxVolume = 1.0f; // hızlıyken volume

    private AudioSource loopSource;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        // Loop için AudioSource ekle
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

        UpdateMotorSound();
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

    private void UpdateMotorSound()
    {
        // Hız değerini normalize et (0 → 1)
        float speed = rb.linearVelocity.magnitude;
        float t = Mathf.InverseLerp(0f, maxSpeed, speed);

        // Pitch ve volume'ü bu değere göre ayarla
        loopSource.pitch = Mathf.Lerp(minPitch, maxPitch, t);
        loopSource.volume = Mathf.Lerp(minVolume, maxVolume, t);

        // Eğer loop çalmıyorsa başlat
        if (!loopSource.isPlaying)
            loopSource.Play();
    }

    public void SetActive(bool state)
    {
        isActive = state;

        if (!state)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            loopSource.Stop();
        }
    }

    public bool IsStopped()
    {
        return rb.linearVelocity.magnitude < 0.1f;
    }
}
