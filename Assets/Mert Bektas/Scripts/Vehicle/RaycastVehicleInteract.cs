using UnityEngine;

public class RaycastVehicleInteract : MonoBehaviour
{
    [Header("Ayarlar")]
    public Camera playerCamera;
    public float interactDistance = 3f;
    public KeyCode interactKey = KeyCode.F;

    [Header("Referanslar")]
    public PlayerMovement playerMovement; // mevcut player hareket scriptin
    public Transform seatPoint;
    public Transform exitPoint;

    private VehicleController2 currentVehicle;
    private Rigidbody playerRb;
    private Collider playerCollider;
    private bool isInVehicle = false;

    private void Awake()
    {
        playerRb = GetComponent<Rigidbody>();
        playerCollider = GetComponent<Collider>();
    }

    void Update()
    {
        // F tuşuna basıldığında
        if (Input.GetKeyDown(interactKey))
        {
            if (!isInVehicle)
            {
                TryEnterVehicle();
            }
            else
            {
                // içindeyken çıkmak
                if (currentVehicle != null && currentVehicle.IsStopped())
                {
                    ExitVehicle();
                }
            }
        }

        // Koltukta sabitleme
        if (isInVehicle && seatPoint != null)
        {
            transform.position = seatPoint.position;
            transform.rotation = seatPoint.rotation;
        }
    }

    private void TryEnterVehicle()
    {
        // Kamera yönünden ray gönder
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance))
        {
            // Araç kontrolü
            VehicleController2 vehicle = hit.collider.GetComponentInParent<VehicleController2>();
            if (vehicle != null)
            {
                currentVehicle = vehicle;
                EnterVehicle();
            }
        }
    }

    private void EnterVehicle()
    {
        isInVehicle = true;
        playerMovement.enabled = false;

        // Player fiziklerini kapat
        playerRb.isKinematic = true;
        playerCollider.enabled = false;

        // Koltuğa taşı
        transform.position = seatPoint.position;
        transform.rotation = seatPoint.rotation;

        // Aracı aktif et
        currentVehicle.SetActive(true);
    }

    private void ExitVehicle()
    {
        isInVehicle = false;
        playerMovement.enabled = true;

        // Player fiziklerini geri aç
        playerRb.isKinematic = false;
        playerCollider.enabled = true;

        // Oyuncuyu exit noktasına koy
        transform.position = exitPoint.position;
        transform.rotation = exitPoint.rotation;

        // Aracı devre dışı bırak
        if (currentVehicle != null)
        {
            currentVehicle.SetActive(false);
            currentVehicle = null;
        }
    }
}
