using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

//TODO: Probably update to localstorage
public static class SaveManager
{
    private const string SaveKey = "IfGameSave";

    public static void SaveGame(GameState gameState)
    {

        BinaryFormatter formatter = new BinaryFormatter();
        MemoryStream memoryStream = new MemoryStream();
        formatter.Serialize(memoryStream, gameState);
        string dataToSave = Convert.ToBase64String(memoryStream.ToArray());
        PlayerPrefs.SetString(SaveKey, dataToSave);
        PlayerPrefs.Save();
        LoggingUtil.Log("Game saved successfully.");
    }

    public static GameState LoadGame()
    {
        GameState dataToReturn;
        if (PlayerPrefs.HasKey(SaveKey))
        {
            string dataToLoad = PlayerPrefs.GetString(SaveKey);
            if (!string.IsNullOrEmpty(dataToLoad))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(dataToLoad));
                try
                {
                    dataToReturn = (GameState)formatter.Deserialize(memoryStream);
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
            LoggingUtil.Log("Game loaded successfully.");
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
