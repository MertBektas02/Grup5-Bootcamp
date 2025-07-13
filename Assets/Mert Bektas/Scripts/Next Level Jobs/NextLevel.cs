using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
    [SerializeField] private LevelEntryCostData levelData;

    public void TryEnterNextLevel()
    {
        if (ResourceManager.Instance.TrySpendResources(levelData.costList))
        {
            SceneManager.LoadScene(sceneName:"EmptySceneMB");
            //SceneManager.LoadScene(levelData.sceneName);
        }
        else
        {
            Debug.Log("Yeterli kaynağın yok!");
        }
    }
}
