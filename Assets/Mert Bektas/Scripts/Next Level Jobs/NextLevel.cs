using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
    [SerializeField] private LevelEntryCostData levelData;

    public void TryEnterNextLevel()
    {
        if (ResourceManager.Instance.TrySpendResources(levelData.costList))
        {
            SoundManager.PlaySound(SoundType.Purchased);
            var dataManager = FindFirstObjectByType<DataPersistenceManager>();
            if (dataManager != null)
            {
                dataManager.ResetSaveData();
                // dataManager.NewGame();
                // dataManager.SaveGame();
            }
            SceneManager.LoadScene(sceneName: "Level1MAIN");
            //SceneManager.LoadScene(levelData.sceneName);
        }
        else
        {
            NotificationManager.Instance.ShowNotification("Not enough Resources!", 2f, SoundType.Denied);
            SoundManager.PlaySound(SoundType.Denied);
        }
    }
}
