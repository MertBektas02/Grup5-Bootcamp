using UnityEngine;
using TMPro;

public class CurrentHordeUIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI hordeMessageText;
    [SerializeField] private float displayDuration = 5f;

    private Coroutine messageRoutine;

    public void DisplayCurrentHordeDays()
    {
        int currentDay = FindFirstObjectByType<GameTimeManager>().currentDay;
        int hordeDay = FindFirstObjectByType<HordeManager>().hordeDay;

        int daysLeft = hordeDay - currentDay;

        string message;

        if (daysLeft > 0)
        {
            message = $"Horde yaklaşıyor. {daysLeft} gün kaldı!";
            SoundManager.PlaySound(SoundType.CurrentDaySFX);
        }
        else if (daysLeft == 0)
        {
            message = $"‼ Horde bugün geliyor!";
            SoundManager.PlaySound(SoundType.CurrentDaySFX);
        }
        else
        {
            return; // Artık geçmiş, mesaj göstermeyelim.
        }

        ShowMessage(message);
    }

    private void ShowMessage(string message)
    {
        if (messageRoutine != null)
            StopCoroutine(messageRoutine);

        messageRoutine = StartCoroutine(DisplayRoutine(message));
    }

    private System.Collections.IEnumerator DisplayRoutine(string message)
    {
        hordeMessageText.text = message;
        hordeMessageText.gameObject.SetActive(true);

        yield return new WaitForSeconds(displayDuration);

        hordeMessageText.gameObject.SetActive(false);
    }
}