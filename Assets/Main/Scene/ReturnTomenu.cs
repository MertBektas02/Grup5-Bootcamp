using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnTomenu : MonoBehaviour
{
    public void ReturnToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
