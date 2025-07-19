using UnityEngine;
using UnityEngine.UI;

public class RequirementsListUI : MonoBehaviour
{
    [System.Serializable]
    public class ResourceRequirement
    {
        public string name;
        public int current;
        public int required;
    }

    public ResourceRequirement[] requirements = new ResourceRequirement[5];

    public Text[] itemsText;         // RequirementsList/ItemsText içindeki 5 Text objesi
    public GameObject[] itemsStatus; // RequirementsList/ItemsStatus içindeki 5 tik işareti (Image veya GameObject)

    void Start()
    {
        UpdateRequirementsUI();
    }

    public void UpdateRequirementsUI()
    {
        for (int i = 0; i < requirements.Length; i++)
        {
            var req = requirements[i];

            itemsText[i].text = $"{req.name}: {req.current} / {req.required}";
            itemsStatus[i].SetActive(req.current >= req.required);
        }
    }
}
