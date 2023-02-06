/*
 * Written by:
 * Henrik
 * 
 */

using UnityEngine;
using System.Linq;
using System;
using UnityEditor;

[CreateAssetMenu(menuName = "A_Gods_Draw/BoardElementClassNames"), System.Serializable]
public class BoardElementClassNames : ScriptableObject
{
    public static BoardElementClassNames instance;

    public string[] Names; 

    public int getIndexOf(string name)
    {
        for (int i = 0; i < Names.Length; i++)
        {
            if(name.Equals(Names[i]))
                return i;
        }
        return -1;
    }

    private void Awake() {
        if(instance == null)
            instance = this;
    }

    #if UNITY_EDITOR

    private void OnValidate() {
        if(instance == null)
            instance = this;

        UpdateEnemyList();
    }

    public void UpdateEnemyList()
    {
        // # Found this code at: https://stackoverflow.com/questions/857705/get-all-derived-types-of-a-type
        // gets a list of all classes derived by class typeof(IMonster)
        var listOfBs = AppDomain.CurrentDomain.GetAssemblies()
            // alternative: .GetExportedTypes()
            .SelectMany(domainAssembly => domainAssembly.GetTypes())
            .Where(type => typeof(BoardElement).IsAssignableFrom(type)
            // alternative: => type.IsSubclassOf(typeof(B))
            // alternative: && type != typeof(B)
            // alternative: && ! type.IsAbstract
            ).ToArray();
        
        Names = new string[listOfBs.Length+1];
        for (int i = 0; i < listOfBs.Length; i++)
            Names[i+1] = listOfBs[i].Name;

        Names[0] = "None";
    } 
    #endif
}
