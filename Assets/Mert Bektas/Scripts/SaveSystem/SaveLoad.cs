using UnityEngine;

public class SaveLoad : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            DataPersistenceManager.Instance.SaveGame();
            //Debug.Log("Manual Save triggered (F5).");
        }

        if (Input.GetKeyDown(KeyCode.F9))
        {
            DataPersistenceManager.Instance.LoadGame();
            //Debug.Log("Manual Load triggered (F9).");
        }
    }

    public void SaveWithButton()
    {
        DataPersistenceManager.Instance.SaveGame();
        NotificationManager.Instance.ShowNotification("Game saved!", 2f, SoundType.GameSaved);
        SoundManager.PlaySound(SoundType.GameSaved);
    }
    public void LoadWithButton()
    {
        DataPersistenceManager.Instance.LoadGame();
        NotificationManager.Instance.ShowNotification("Game Loaded!");


    }
}
