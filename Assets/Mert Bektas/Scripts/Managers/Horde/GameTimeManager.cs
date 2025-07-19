using UnityEngine;
using System;

public class GameTimeManager : MonoBehaviour, IDataPersistence
{
    public int currentDay = 1;
    public float dayDuration = 1200f;
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
        //Debug.Log(currentTime);
    }

    public float GetDayProgress01()
    {
        return currentTime / dayDuration; // 0-1 arası gün ilerleme oranı
    }

    //--------Save, Load--------
    public void SaveData(ref GameData data)
    {
        data.currentDayData = currentDay;
        data.currentTimeData = currentTime;
    }

    public void LoadData(GameData data)
    {
        currentDay = data.currentDayData;
        currentTime = data.currentTimeData;
    }
}