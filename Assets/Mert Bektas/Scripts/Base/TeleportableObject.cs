using UnityEngine;

public class TeleportableObject : MonoBehaviour
{
    public enum TeleportType { ToBase, ToOutside }
    public TeleportType teleportType;

    public Transform targetPosition;

    [Header("UI")]
    public GameObject interactionUI; // World Space UI

    public void TeleportPlayer(GameObject player)
    {
        player.transform.position = targetPosition.position;
        player.transform.rotation = targetPosition.rotation;

        //BaseTeleportManager.Instance.OnTeleport(teleportType);
    }

    public void ShowUI(bool show)
    {
        if (interactionUI != null)
            interactionUI.SetActive(show);
        //Debug.Log(show);
    }
}