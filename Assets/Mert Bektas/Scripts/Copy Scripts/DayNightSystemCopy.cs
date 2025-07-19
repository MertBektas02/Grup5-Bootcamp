using UnityEngine;

public class DayNightSystemCopy : MonoBehaviour
{
    [Header("Referanslar")]
    public GameTimeManager timeManager; // Inspector’dan bağla
    public Light sun;

    [Header("Oranlar (0-1 toplamı 1 olacak)")]
    [Range(0.1f, 0.9f)] public float dayPortion = 0.7f; // örnek: %70 gündüz, %30 gece
    [Range(0.1f, 0.9f)] public float nightPortion = 0.3f; // bu otomatik hesaplanabilir

    [Header("Güneş Ayarları")]
    public Gradient sunColorOverTime;
    public AnimationCurve sunIntensityCurve;

    [Header("Durum Bool'ları (Sadece Okunur)")]
    [SerializeField] private bool isDay = false;
    [SerializeField] private bool isNight = false;

    private void Update()
    {
        if (timeManager == null)
        {
            Debug.LogWarning("[DayNight] TimeManager atanmamış!");
            return;
        }
        if (sun == null)
        {
            Debug.LogWarning("[DayNight] Sun Light atanmamış!");
            return;
        }

        // 0-1 arası gün ilerleme oranı
        float t = timeManager.GetDayProgress01();

        // test için log
        Debug.Log($"[DayNight] DayProgress: {t:F3} | CurrentDay: {timeManager.currentDay}");

        if (t < dayPortion)
        {
            if (!isDay) Debug.Log("[DayNight] Gündüz başladı!");
            isDay = true;
            isNight = false;

            float normalized = Mathf.InverseLerp(0f, dayPortion, t) * 0.5f;
            UpdateSun(normalized);
        }
        else
        {
            if (!isNight) Debug.Log("[DayNight] Gece başladı!");
            isDay = false;
            isNight = true;

            float normalized = Mathf.InverseLerp(dayPortion, 1f, t) * 0.5f + 0.5f;
            UpdateSun(normalized);
        }
    }

    void UpdateSun(float timeOfDay)
    {
        // Zamanı logla
        Debug.Log($"[DayNight] timeOfDay: {timeOfDay:F3}");

        // ışık rotasyonu
        float sunAngle = (timeOfDay * 360f) - 90f;
        sun.transform.rotation = Quaternion.Euler(sunAngle, 170f, 0f);

        // ışık rengi ve yoğunluğu
        if (sunColorOverTime != null)
            sun.color = sunColorOverTime.Evaluate(timeOfDay);
        if (sunIntensityCurve != null)
            sun.intensity = sunIntensityCurve.Evaluate(timeOfDay);

        // ortam ışığı
        RenderSettings.ambientLight = sun.color * 0.4f;
    }
}
