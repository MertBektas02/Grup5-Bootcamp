using System.IO;
using UnityEngine;
using System;

public class FileDataHandler
{
    private string fullPath;

    public FileDataHandler(string dataDirPath, string fileName)
    {
        fullPath = Path.Combine(dataDirPath, fileName);
    }

    public void Save(GameData data)
    {
        try
        {
            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(fullPath, json);
            Debug.Log("Game saved to " + fullPath);
        }
        catch (Exception e)
        {
            Debug.LogError("Error saving data: " + e);
        }
    }

    public GameData Load()
    {
        if (!File.Exists(fullPath))
        {
            Debug.LogWarning("No save file found at " + fullPath);
            return null;
        }

        try
        {
            string json = File.ReadAllText(fullPath);
            return JsonUtility.FromJson<GameData>(json);
        }
        catch (Exception e)
        {
            Debug.LogError("Error loading data: " + e);
            return null;
        }
    }
}
