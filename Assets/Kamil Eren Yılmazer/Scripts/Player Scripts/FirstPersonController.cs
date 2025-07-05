using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FirstPersonController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float mouseSensitivity = 2f;
    public float jumpHeight = 1.5f;
    public float gravity = -9.81f;
    public Transform cameraPivot;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private float cameraPitch = 0f;

    public float groundCheckDistance = 0.4f;
    public LayerMask groundMask;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        HandleMovement();
        HandleMouseLook();
    }

    void HandleMovement()
    {
        isGrounded = Physics.CheckSphere(transform.position + Vector3.down * (controller.height / 2 - controller.radius + 0.1f), groundCheckDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Yer çekimini sabitlemek için
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * moveSpeed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        cameraPitch -= mouseY;
        cameraPitch = Mathf.Clamp(cameraPitch, -35f, 35f);

        cameraPivot.localRotation = Quaternion.Euler(cameraPitch, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }
}

