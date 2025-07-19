
using UnityEngine;
using UnityEngine.UI;

public class NextLevelUI : MonoBehaviour
{
    public Text requirementsText;
    public Button nextLevelButton;

    public void TryNextLevel()
    {
        Debug.Log("Next level attempted.");
        requirementsText.text = "Proceeding to next level...";
    }
}
