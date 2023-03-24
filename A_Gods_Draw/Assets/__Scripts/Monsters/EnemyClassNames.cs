/*
 * Written by:
 * Henrik
 * 
 */

using UnityEngine;
using System.Linq;
using System;


[CreateAssetMenu(menuName = "A_Gods_Draw/EnemyClassNames"), System.Serializable]
public class EnemyClassNames : ScriptableObject
{
    public static EnemyClassNames instance;

    public string[] Names; 

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
            .Where(type => typeof(Monster).IsAssignableFrom(type)
            // alternative: => type.IsSubclassOf(typeof(B))
            // alternative: && type != typeof(B)
            // alternative: && ! type.IsAbstract
            ).ToArray();
        
        Names = new string[listOfBs.Length+1];
        for (int i = 0; i < listOfBs.Length; i++)
            Names[i+1] = listOfBs[i].Name;

        Names[0] = "Any";
    } 
    #endif
}
