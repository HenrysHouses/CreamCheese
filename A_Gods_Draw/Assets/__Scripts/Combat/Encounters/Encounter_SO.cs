using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct EnemyData
 {
   public  GameObject enemy;
   public Vector3 enemyPos;
 }

[CreateAssetMenu(menuName = "Encounters")]

public class Encounter_SO : ScriptableObject
{
    public BattlefieldID battlefieldID;
    public List<EnemyData> enemies = new List<EnemyData>();

    

}
