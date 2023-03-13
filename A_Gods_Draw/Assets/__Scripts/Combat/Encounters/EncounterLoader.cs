/* 
 * Written by 
 * Henrik
*/

using System.Collections.Generic;
using UnityEngine;

public enum EncounterDifficulty
{
    Easy,
    Medium,
    Hard,
    elites,
    Boss,
    Any,
    Tutorial
}

/// <summary>
/// Encounter loader manager script which loads the encounters in combat
/// </summary>
class EncounterLoader : MonoBehaviour
{
    /// <summary>Loads all encounters from Assets/Resources/Encounters/ + Encounter Difficulty</summary>
    /// <param name="difficulty">Limit which encounters to load, Any loads all encounters</param>
    /// <returns>All Encounters found</returns>
    static public Encounter_SO[] LoadAllEncountersOf(EncounterDifficulty difficulty)
    {
        Encounter_SO[] loaded = null;
        if(difficulty == EncounterDifficulty.Any)
        {
            loaded = Resources.LoadAll<Encounter_SO>("Encounters/");
        }
        else
            loaded = Resources.LoadAll<Encounter_SO>("Encounters/" + difficulty.ToString());

        if(loaded == null)
            throw new UnityException("Was not able to load any encounters in: \"Assets/Resources/Encounters/" + difficulty + "\"");
        return loaded;
    }

    /// <summary>Instantiates encounter enemies under the specified parent</summary>
    /// <param name="Encounter">Encounter to spawn</param>
    /// <param name="EnemyParent">Parent Transform for instantiated enemies</param>
    /// <returns>Base class array of all spawned Monsters</returns>
    static public Monster[] InstantiateEncounter(Encounter_SO Encounter, Transform EnemyParent)
    {
        // Spawn Encounter
        List<Monster> initEnemies = new List<Monster>();
        for (int i = 0; i < Encounter.enemies.Count; i++)
        {
            GameObject spawn = Instantiate(Encounter.enemies[i].enemy,Encounter.enemies[i].enemyPos,Quaternion.identity);
            spawn.transform.SetParent(EnemyParent, false);
            initEnemies.Add(spawn.GetComponent<Monster>());
        }
        return initEnemies.ToArray();
    }

    /// <summary>Instantiates a random encounter enemies under the specified parent</summary>
    /// <param name="Encounter">Possible Encounters</param>
    /// <param name="EnemyParent">Parent Transform for instantiated enemies</param>
    /// <returns>Base class array of all spawned Monsters</returns>
    static public Monster[] InstantiateRandomEncounter(Encounter_SO[] Encounter, Transform EnemyParent, out Encounter_SO chosenEncounter)
    {
        // Get Random Encounter
        chosenEncounter = Encounter[UnityEngine.Random.Range(0,Encounter.Length-1)];
        
        return InstantiateEncounter(chosenEncounter, EnemyParent);
    }
}