using System.Collections;
using UnityEngine;
public class PlayerBaseInteractor : MonoBehaviour
{
    private TeleportableObject currentTeleportable;

    private bool isTeleporting = false;


    private void Update()
    {
        DoorInteraction();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out TeleportableObject teleportable))
        {
            currentTeleportable = teleportable;
            teleportable.ShowUI(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out TeleportableObject teleportable) && teleportable == currentTeleportable)
        {
            teleportable.ShowUI(false);
            currentTeleportable = null;
        }
    }
    private void DoorInteraction()
    {
        if (currentTeleportable != null && Input.GetKeyDown(KeyCode.F) && !isTeleporting)
        {
            isTeleporting = true;
            currentTeleportable.TeleportPlayer(gameObject);
            StartCoroutine(ResetTeleportFlagAfterDelay(0.5f)); // Cooldown süresi
        }
    }

    private IEnumerator ResetTeleportFlagAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        isTeleporting = false;
    }
}
