using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveableItemSet : MonoBehaviour
{
    //! This checks if an item exists in that set/collection or not
    // reasoning of using hashset is because it only cares if the items exists and if it can make a reference to it if it does
    public HashSet<string> SavedItems { get; private set; } = new HashSet<string>();

    private void Awake()
    {
        SaveEvents.SaveInitiated += Save;
        Load();
    }

    void Save()
    {
        TEST_SaveJson.Save("SavedItems", SavedItems);
    }

    void Load()
    {
        if (TEST_SaveJson.SaveExists("SavedItems"))
        {
            SavedItems = TEST_SaveJson.Load<HashSet<string>>("SavedItems");
        }
    }
}
