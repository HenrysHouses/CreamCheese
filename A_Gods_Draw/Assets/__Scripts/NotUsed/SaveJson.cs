using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class GameSaver
{
    private static DeckList_SO deckList;

    private static string path = "";
    private static string persistantPath = "";

    private static void CreatePlayerData()
    {
        deckList = DeckManager_SO.getStarterDeck();
    }

    public static void InitializeSaving()
    {
        CreatePlayerData();
        SetPaths();
        Debug.Log("AAAA setpaths plz!");
    }

    private static void SetPaths()
    {
        path = Application.dataPath + Path.AltDirectorySeparatorChar + "SaveData.json";
        persistantPath = Application.persistentDataPath + Path.AltDirectorySeparatorChar + "SaveData.json";

        if (!Directory.Exists(persistantPath))
        {
            string savePath = persistantPath;
            using StreamWriter writer = new StreamWriter(savePath);

            Debug.Log("file created?");
        }
    }

    public static void SaveData()
    {
        string savePath = persistantPath; // <- path for us to see in assets, persistantPath is what should be used
        string json = JsonUtility.ToJson(deckList);

        using StreamWriter writer = new StreamWriter(savePath);
        writer.Write(json);
    }

    public static DeckList_SO LoadData()
    {
        using StreamReader streamReader = new StreamReader(persistantPath); // <- path for us to see in assets, persistantPath is what should be used
        string json = streamReader.ReadToEnd();

        DeckList_SO deck = JsonUtility.FromJson<DeckList_SO>(json);

        return deck;
    }
}
