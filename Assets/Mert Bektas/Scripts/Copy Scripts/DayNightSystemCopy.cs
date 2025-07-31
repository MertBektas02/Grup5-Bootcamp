using UnityEngine;
using System.Collections;

public class DayNightSystemCopy : MonoBehaviour
{
    [Header("Referanslar")]
    public GameTimeManager timeManager;
    public Light sun;

    [Header("Oranlar (0-1 toplamı 1 olacak)")]
    [Range(0.1f, 0.9f)] public float dayPortion = 0.7f;
    [Range(0.1f, 0.9f)] public float nightPortion = 0.3f;

    [Header("Güneş Ayarları")]
    public Gradient sunColorOverTime;
    public AnimationCurve sunIntensityCurve;

    [Header("Durum Bool'ları (Sadece Okunur)")]
    [SerializeField] private bool isDay = false;
    [SerializeField] private bool isNight = false;

    public AmbienceManager2 ambienceManager;
    private bool lastIsNight = false;

    // 🟢 Getter methodları (public)
    public bool GetIsNight() => isNight;
    public bool GetIsDay() => isDay;

    void Start()
    {
        StartCoroutine(InitAmbienceAfterFrame());
    }

    private IEnumerator InitAmbienceAfterFrame()
    {
        yield return null;

        if (ambienceManager != null)
        {
            ambienceManager.SetNightMode(isNight);
            lastIsNight = isNight;
        }
    }

    void Update()
    {
        if (timeManager == null || sun == null) return;

        float t = timeManager.GetDayProgress01();
        float shifted = (t + 0.25f) % 1f;

        isDay = shifted >= 0.25f && shifted < 0.75f;
        isNight = !isDay;

        UpdateSun(shifted);

        if (isNight != lastIsNight)
        {
            if (ambienceManager != null)
            {
                ambienceManager.SetNightMode(isNight);
            }
            lastIsNight = isNight;
        }
    }

    void UpdateSun(float timeOfDay)
    {
        float sunAngle = (timeOfDay * 360f) - 90f;
        sun.transform.rotation = Quaternion.Euler(sunAngle, 170f, 0f);

        if (sunColorOverTime != null)
            sun.color = sunColorOverTime.Evaluate(timeOfDay);

        if (sunIntensityCurve != null)
            sun.intensity = sunIntensityCurve.Evaluate(timeOfDay);

        RenderSettings.ambientLight = sun.color * 0.4f;
    }
}
