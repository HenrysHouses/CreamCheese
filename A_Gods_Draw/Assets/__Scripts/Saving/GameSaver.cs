//charlie
// Edited by Henrik

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class GameSaver
{
    private static string path = "";
    private static string persistantPath = "";

    public static void InitializeSaving()
    {
        SetPaths();
    }

    private static void SetPaths()
    {
        path = Application.dataPath + Path.AltDirectorySeparatorChar + "SaveData.json";
        persistantPath = Path.Combine(Application.persistentDataPath, "GameSave.json");

        if (Directory.Exists(persistantPath))
        {
            string savePath = persistantPath;
            using StreamWriter writer = new StreamWriter(savePath);

            Debug.Log("file created?");
        }
    }

    public static void SaveData(CardQuantityContainer deck)
    {
        string savePath = persistantPath; // <- path for us to see in assets, persistantPath is what should be used
        string json = JsonUtility.ToJson(deck);

        using StreamWriter writer = new StreamWriter(savePath);
        writer.Write(json);
        
        Debug.Log(json);
    }

    public static void SavePlayerData(PlayerDataContainer _playerData)
    {

        

    } 

    public static DeckListData LoadData()
    {
        using StreamReader streamReader = new StreamReader(persistantPath); // <- path for us to see in assets, persistantPath is what should be used
        string json = streamReader.ReadToEnd();

        CardQuantityContainer deck = JsonUtility.FromJson<CardQuantityContainer>(json);
        bool loadedStarterDeck = false;

        if(deck.Cards == null)
        {
            deck = DeckList_SO.getStarterDeck().deckData.GetDeckData();
            loadedStarterDeck = true;
        }
        else if(deck.Cards.Length == 0)
        {
            deck = DeckList_SO.getStarterDeck().deckData.GetDeckData();
            loadedStarterDeck = true;
        }

        Debug.Log("Loading completed");
        // Debug.Log(json);
        DeckListData loadedDeck = new DeckListData(deck.Cards);
        // CardExperience.ClearUnusedIDs(loadedDeck);

        streamReader.Close();

        if(loadedStarterDeck)
            SaveData(loadedDeck.GetDeckData());

        return loadedDeck;
    }
}
