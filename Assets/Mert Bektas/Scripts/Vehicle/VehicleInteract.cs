using UnityEngine;

public class VehicleInteract : MonoBehaviour
{
    [Header("Player References")]
    public PlayerMovement playerMovement; // kendi scriptin
    public KeyCode interactKey = KeyCode.F;

    [Header("Vehicle")]
    public VehicleController2 currentVehicle;
    public Transform seatPoint;
    public Transform exitPoint;

    private bool isInVehicle = false;

    private Rigidbody playerRb;
    private Collider playerCollider;

    void Awake()
    {
        playerRb = GetComponent<Rigidbody>();
        playerCollider = GetComponent<Collider>();
    }

    void Update()
    {
        if (Input.GetKeyDown(interactKey))
        {
            if (!isInVehicle && currentVehicle != null)
            {
                EnterVehicle();
            }
            else if (isInVehicle)
            {
                // ancak araç duruyorsa
                if (currentVehicle.IsStopped())
                {
                    ExitVehicle();
                }
            }
        }

        if (isInVehicle && seatPoint != null)
        {
            // her frame oyuncuyu koltuğa yapıştır
            transform.position = seatPoint.position;
            transform.rotation = seatPoint.rotation;
        }
    }

    void EnterVehicle()
    {
        isInVehicle = true;
        playerMovement.enabled = false;

        // Player fiziklerini devre dışı bırak
        playerRb.isKinematic = true;
        playerCollider.enabled = false;

        // Oyuncuyu koltuğa taşı
        transform.position = seatPoint.position;
        transform.rotation = seatPoint.rotation;

        currentVehicle.SetActive(true);
    }

    void ExitVehicle()
    {
        isInVehicle = false;
        playerMovement.enabled = true;

        // Player fiziklerini geri aç
        playerRb.isKinematic = false;
        playerCollider.enabled = true;

        // Oyuncuyu exit noktasına koy
        transform.position = exitPoint.position;
        transform.rotation = exitPoint.rotation;

        currentVehicle.SetActive(false);
    }
}
