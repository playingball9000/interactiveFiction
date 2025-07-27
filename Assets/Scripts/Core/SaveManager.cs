using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

//TODO: Probably update to localstorage
public static class SaveManager
{
    private const string SaveKey = "IfGameSave";

    public static void SaveGame(WorldState worldState)
    {

        BinaryFormatter formatter = new();
        MemoryStream memoryStream = new();
        formatter.Serialize(memoryStream, worldState);
        string dataToSave = Convert.ToBase64String(memoryStream.ToArray());
        PlayerPrefs.SetString(SaveKey, dataToSave);
        PlayerPrefs.Save();
        Log.Debug("Game saved successfully.");
    }

    public static WorldState LoadGame()
    {
        WorldState dataToReturn;
        if (PlayerPrefs.HasKey(SaveKey))
        {
            string dataToLoad = PlayerPrefs.GetString(SaveKey);
            if (!string.IsNullOrEmpty(dataToLoad))
            {
                BinaryFormatter formatter = new();
                MemoryStream memoryStream = new(Convert.FromBase64String(dataToLoad));
                try
                {
                    dataToReturn = (WorldState)formatter.Deserialize(memoryStream);
                }
                catch
                {
                    dataToReturn = default;
                }
            }
            else
            {
                dataToReturn = default;
            }
            Log.Debug("Game loaded successfully.");
            return dataToReturn;
        }
        Debug.Log("No save found.");
        return null;
    }

    public static void ClearSave()
    {
        PlayerPrefs.DeleteKey(SaveKey);
        Debug.Log("Save data cleared.");
    }
}
