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
    private Vector3 inputDirection;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Klavye hareketleri
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        inputDirection = transform.right * x + transform.forward * z;

        // Zıplama
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    void FixedUpdate()
    {
        Vector3 move = inputDirection * moveSpeed;
        Vector3 velocity = new Vector3(move.x, rb.linearVelocity.y, move.z);
        rb.linearVelocity = velocity;
    }
}
