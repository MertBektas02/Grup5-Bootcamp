using UnityEngine;
public class HordeManager : MonoBehaviour
{
   
    public int hordeDay = 5;
    private bool playerEscaped = false;
    [Header("Referances")]
    [SerializeField] private CurrentHordeUIManager hordeUI;

    void Start()
    {
        FindFirstObjectByType<GameTimeManager>().OnNewDayStarted += OnNewDayStarted;
    }

    void OnNewDayStarted(int day)
    {
        int daysLeft = hordeDay - day;
        if (daysLeft > 0)
        {
            hordeUI.DisplayCurrentHordeDays();
        }
        else if (daysLeft == 0)
        {
            if (!playerEscaped)
                GameOver();
        }
    }

    public void MarkPlayerEscaped()
    {
        playerEscaped = true;
    }

    void GameOver()
    {
        Debug.Log("GAME OVER! Horde geldi, ama sen kaçamadın.");
        // Oyunu bitir, menü göster, vs.
    }
}