using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 7f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    private Rigidbody rb;
    private Vector3 moveInput;
    private bool isGrounded;

    [Header("Footstep Settings")]
    [SerializeField] private float stepInterval = 0.5f;
    private float stepTimer = 0f;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // Dönmeyi engelle

    }

    void Update()
    {

        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");
        moveInput = (transform.right * x + transform.forward * z).normalized;

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z); // Dikey hızı sıfırla
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
        //HandleFootsteps();
    }

    void FixedUpdate()
    {
        Vector3 moveVelocity = moveInput * moveSpeed;
        rb.linearVelocity = new Vector3(moveVelocity.x, rb.linearVelocity.y, moveVelocity.z);
    }

    void HandleFootsteps()
    {
        Vector2 horizontalVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.z);
        bool isMoving = horizontalVelocity.magnitude > 0.1f;

        // Karakter yerde ve yürüyor mu kontrolü
        if (isGrounded && isMoving)
        {
            stepTimer += Time.deltaTime;

            if (stepTimer >= stepInterval)
            {
                SoundManager.PlaySound(SoundType.Footstep);
                stepTimer = 0f;
            }
        }
        else
        {
            stepTimer = 0f;
        }
    }
}