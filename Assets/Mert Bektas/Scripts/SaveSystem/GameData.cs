using System;
using System.Collections.Generic;

[Serializable]
public class GameData
{
    // Kaynaklar (örn. odun, taş, demir vs.)
    public Dictionary<string, int> resourceAmounts = new();

    // Sahnedeki nesnelerin toplanma durumu (örnek: ağaç, taş vs.)
    public List<string> collectedObjectIDs = new();

    // Yapılan upgrade’ler
    public Dictionary<string, bool> unlockedUpgrades = new();

    // Oyuncu pozisyonu (isteğe bağlı kullanılabilir)
    public float[] playerPosition = new float[3];

    // Boş constructor yeni oyun içindir
    public GameData() { }
}
