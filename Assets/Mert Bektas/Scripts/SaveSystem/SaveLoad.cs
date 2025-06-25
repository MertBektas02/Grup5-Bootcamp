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
}
