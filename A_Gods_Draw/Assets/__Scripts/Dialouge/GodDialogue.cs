using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;


[CreateAssetMenu(menuName = "DialogueBox/God Dialogue")]
public class GodDialogue : Dialogue_ScriptableObject
{
    public GodDialogueTrigger trigger;
    public bool GenericTrigger = true;
    [HideInInspector] public Card_SO cardTrigger;
    [HideInInspector] public float chanceToPlay;
    [HideInInspector] static public string[] EnemyClassNames;
    [HideInInspector] public int enemyTrigger;

    /// <summary>Checks if the dialogue should trigger</summary>
    /// <param name="rand">the random % to check</param>
    public bool checkChance(float rand)
    {
        if(rand < chanceToPlay)
            return true;
        return false;
    }

    private void OnValidate() {
        UpdateEnemyList();
    }

    private void UpdateEnemyList()
    {
        // # Found this code at: https://stackoverflow.com/questions/857705/get-all-derived-types-of-a-type
        // gets a list of all classes derived by class typeof(IMonster)
        var listOfBs = AppDomain.CurrentDomain.GetAssemblies()
            // alternative: .GetExportedTypes()
            .SelectMany(domainAssembly => domainAssembly.GetTypes())
            .Where(type => typeof(IMonster).IsAssignableFrom(type)
            // alternative: => type.IsSubclassOf(typeof(B))
            // alternative: && type != typeof(B)
            // alternative: && ! type.IsAbstract
            ).ToArray();
        
        EnemyClassNames = new string[listOfBs.Length];
        for (int i = 0; i < listOfBs.Length; i++)
            EnemyClassNames[i] = listOfBs[i].Name;

        EnemyClassNames[0] = "Any";
    }
}

public enum GodDialogueTrigger
{
    SeeEnemy = 0,
    EnemyKill = 1,
    Hurt = 2,
    Dying = 3,
    Draw = 4,
    Played = 5,
    Discard = 6,
    Shuffle = 7
}