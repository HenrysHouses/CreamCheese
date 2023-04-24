using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using EnemyAIEnums;

[System.Serializable]
public class ActionSelection
{
    
    public EnemyIntent ActionType;
    public ActionConditions[] ActionConditions;
    [Tooltip("Set to true if all conditions has to be met for the action to be valid")]
    public bool AllRequired;
    public int MinStrength, MaxStrength;
    public bool UseStrengthMod;
    public ActionConditions[] StrengthModConditions;
    [Tooltip("If all conditions has to be met for the mod to be applied")]
    public bool AllRequiredForMod;
    [Tooltip("If the condition above is met, the strenght will be set to this value")]
    public int ModifiedStrength;
    public bool UseWeigthMod;
    public ActionConditions[] WeigthModConditions;
    [Tooltip("If all conditions has to be met for the mod to be applied")]
    public bool AllRequiredForWeigthMod;
    [Tooltip("If the increased weight be removed when condition isn't met")]
    public bool ClearOnConditionFalse;
    [Tooltip("How much weight to add/remove")]
    public int ModifiedWeigth;
    public int AddedWeigth;
    [Tooltip("Determines the chance of this action happening when 2 or more actions of same priority is possible")]
    public int Weigth;
    [Tooltip("Determines what actions take presedence when several actions are possible, use weight for randomized selection within set priority level")]
    public int Priority;
    [Tooltip("How many turns this action takes to perform, as in, if 1, will not happen first turn")]
    public int TurnsToPerform;
    public MonsterAction Action;
    public EventReference ActionSFX;

}