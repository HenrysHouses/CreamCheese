/* 
 * Written by 
 * Henrik
*/

using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Data container for enemy prefab and spawn position.
/// </summary>
[System.Serializable]
public struct EnemyData
 {
   public  GameObject enemy;
   public Vector3 enemyPos;
 }

/// <summary>
/// ScriptableObject for containing encounter data.
/// </summary>
[CreateAssetMenu(menuName = "Encounters")]
public class Encounter_SO : ScriptableObject
{
    public BattlefieldID battlefieldID;
    public List<EnemyData> enemies = new List<EnemyData>();
}
