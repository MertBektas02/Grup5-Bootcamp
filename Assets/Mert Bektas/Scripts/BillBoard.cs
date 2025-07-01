using UnityEngine;

public class BillboardYLocked : MonoBehaviour
{
    private Camera mainCamera;
    private Rigidbody rb;
    private bool isGrounded = false;
    private float groundYPosition;

    void Start()
    {
        mainCamera = Camera.main;
        rb = GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision collision)
    {
        // Ground tag'ına çarptığında Y pozisyonunu kilitle
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            groundYPosition = transform.position.y;
            rb.linearVelocity = Vector3.zero; // Düşme hareketini durdur
            rb.isKinematic = true; // Fizik etkileşimini kapat
        }
    }

    void LateUpdate()
    {
        if (mainCamera == null) return;

        // Billboard efekti (Y eksenini yok sayarak kamera yönüne dön)
        Vector3 lookDirection = mainCamera.transform.forward;
        lookDirection.y = 0;
        if (lookDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(lookDirection, Vector3.up);
        }

        // Yere düştükten sonra Y pozisyonunu sabitle
        if (isGrounded)
        {
            transform.position = new Vector3(
                transform.position.x,
                groundYPosition,
                transform.position.z
            );
        }
    }
}