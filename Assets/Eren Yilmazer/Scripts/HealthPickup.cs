using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public float healAmount = 25f;
    public KeyCode interactKey = KeyCode.E;
    private bool isPlayerNearby = false;
    private PlayerHealth playerHealth;

    void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(interactKey))
        {
            if (playerHealth != null)
            {
                playerHealth.RestoreHealth(healAmount);
                Destroy(gameObject); // Objeyi yok et
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerHealth = other.GetComponent<PlayerHealth>();
            isPlayerNearby = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            playerHealth = null;
        }
    }
}