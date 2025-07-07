using UnityEngine;
using System;

public class GameTimeManager : MonoBehaviour
{
    public int currentDay = 1;
    public float dayDuration = 1200f; // 20 dakika
    private float currentTime;

    public event Action<int> OnNewDayStarted;

    void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime >= dayDuration)
        {
            currentTime = 0;
            currentDay++;
            OnNewDayStarted?.Invoke(currentDay);
        }
    }
}