using UnityEngine;

public class PlayerPositionSaver : MonoBehaviour, IDataPersistence
{
    public void LoadData(GameData data)
    {
        // Eğer pozisyon verisi kayıtlıysa uygula
        if (data.playerPosition != null && data.playerPosition.Length == 3)
        {
            transform.position = new Vector3(
                data.playerPosition[0],
                data.playerPosition[1],
                data.playerPosition[2]
            );
        }
    }

    public void SaveData(ref GameData data)
    {
        Vector3 pos = transform.position;
        data.playerPosition[0] = pos.x;
        data.playerPosition[1] = pos.y;
        data.playerPosition[2] = pos.z;
    }
}