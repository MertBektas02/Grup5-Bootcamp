using System;
using System.Collections.Generic;

[Serializable]
public class GameData
{
    // Kaynaklar (örn. odun, taş, demir vs.)
    //public Dictionary<string, int> resourceAmounts = new();
    public List<ResourceAmount> resourceAmounts = new List<ResourceAmount>();

    // Sahnedeki nesnelerin toplanma durumu (örnek: ağaç, taş vs.)
    public List<string> collectedObjectIDs = new();

    // Yapılan upgrade’ler
    public List<UpgradeState> unlockedUpgrades = new();

    // Oyuncu pozisyonu (isteğe bağlı kullanılabilir)
    public float[] playerPosition = new float[3];

    public PlayerData playerData = new PlayerData();

    // Boş constructor yeni oyun içindir

    public GameData()
    {
        playerData.playerHealth = 100;
        playerData.playerFood = 100;
        playerData.playerWater = 100;
    }
}
