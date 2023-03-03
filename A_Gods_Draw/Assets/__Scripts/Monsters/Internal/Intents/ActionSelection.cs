using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ActionSelection
{
    
    public EnemyIntent ActionType;
    public Conditions[] ActionConditions;
    public int MinStrength, MaxStrength;
    [Tooltip("Determines the chance of this action happening when 2 or more actions of same priority is possible")]
    public int Weigth;
    [Tooltip("Determines what actions take presedence when several actions are possible, use weight for randomized selection within set priority level")]
    public int Priority;
    [Tooltip("Set to true if all conditions has to be met for the action to be valid")]
    public bool AllRequired;
    public MonsterAction Action;

}