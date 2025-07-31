using UnityEngine;

public class StarParticleManager : MonoBehaviour
{
    [SerializeField] private GameObject starParticles;
    [SerializeField] private DayNightSystemCopy dayNightSystem;

    private bool lastIsNight = false;

    private void Update()
    {
        bool isNight = dayNightSystem.GetIsNight(); // HATALI OLAN IsNight() yerine bu

        if (isNight != lastIsNight)
        {
            if (starParticles != null)
                starParticles.SetActive(isNight);

            lastIsNight = isNight;
        }
    }
}
