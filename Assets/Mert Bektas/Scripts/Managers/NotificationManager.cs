using System.Collections;
using UnityEngine;
using TMPro;

public class NotificationManager : MonoBehaviour
{
    public static NotificationManager Instance { get; private set; }

    [Header("UI Components")]
    [SerializeField] private GameObject notificationPanel;
    [SerializeField] private TextMeshProUGUI notificationText;

    private Coroutine currentNotification;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        if (notificationPanel != null)
            notificationPanel.SetActive(false);
    }

    /// <summary>
    /// Basit bildirim gösterimi. İstersen ses tipini belirtebilirsin.
    /// </summary>
    public void ShowNotification(string message, float duration = 2f, SoundType? sound = null)
    {
        if (currentNotification != null)
            StopCoroutine(currentNotification);

        currentNotification = StartCoroutine(ShowRoutine(message, duration));


    }

    private IEnumerator ShowRoutine(string message, float duration)
    {
        notificationText.text = message;
        notificationPanel.SetActive(true);

        yield return new WaitForSeconds(duration);

        notificationPanel.SetActive(false);
    }
}
