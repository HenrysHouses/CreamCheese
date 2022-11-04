using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
public class TEST_SaveJson : MonoBehaviour
{
    //Can define the type we are working with as long as its serializable
    public static void Save<T>(string key, T objectToSave)
    {
        string path = Application.persistentDataPath + "/saves/";
        Directory.CreateDirectory(path);
        BinaryFormatter formatter = new BinaryFormatter();
        //takes what we need and as long as we need it nothing bad will happen, but when its no longer needed it disposes of it
        using (FileStream fileStream = new FileStream(path + key + ".json", FileMode.Create))
        {
            formatter.Serialize(fileStream, objectToSave);
        }
        Debug.Log("saved! using test script");
    }

    public static T Load<T>(string key) //the key = what file we are looking for
    {
        string path = Application.persistentDataPath + "/saves/";

        BinaryFormatter formatter = new BinaryFormatter();
        T returnValue = default(T); //if we dont find a file that has data in it for this load, then it gives the defaul for that type

        //takes what we need and as long as we need it nothing bad will happen, but when its no longer needed it disposes of it
        using (FileStream fileStream = new FileStream(path + key + ".json", FileMode.Open))
        {
            returnValue = (T)formatter.Deserialize(fileStream);
        }

        return returnValue;
    }

    /*checks if a save already exists for the object / for a key
     * this is to check if there is anything to load / anything to try to load
     * this way there wont be any error on trying to load a file that doesnt exist*/
    public static bool SaveExists(string key)
    {
        string path = Application.persistentDataPath + "/saves/" + key + ".txt";
        return File.Exists(path);
    }

    //!to delete all save progress (good for testing me thinks)
    public static void DeleteAllSaveFiles()
    {
        string path = Application.persistentDataPath + "/saves/";
        DirectoryInfo directory = new DirectoryInfo(path);
        directory.Delete();
        Directory.CreateDirectory(path);
    }
}
