/* 
 * Written by 
 * Henrik
*/

using UnityEngine;

/// <summary>Extension to define when a god should trigger its dialogue</summary>
[CreateAssetMenu(menuName = "DialogueBox/God Dialogue")]
public class GodDialogue : Dialogue_ScriptableObject
{
    public GodDialogueTrigger trigger;
    public bool GenericTrigger = true;
    [HideInInspector] public Card_SO cardTrigger;
    [HideInInspector] public float chanceToPlay;
    [HideInInspector] public int enemyTrigger;
    public EnemyClassNames enemyClassNames;

    /// <summary>Checks if the dialogue should trigger</summary>
    /// <param name="rand">the random % to check</param>
    public bool checkChance(float rand)
    {
        if(rand < chanceToPlay)
            return true;
        return false;
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