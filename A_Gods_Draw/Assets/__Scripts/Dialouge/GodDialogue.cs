using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "DialogueBox/God Dialogue")]
public class GodDialogue : Dialogue_ScriptableObject
{
    public GodDialogueTrigger trigger;
    public bool GenericTrigger = true;
    [HideInInspector] public Card_SO cardTrigger;
    [HideInInspector] public float chanceToPlay;

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
    EnterBossFight,
    BossKill,
    Hurt,
    Dying,
    Draw,
    Played,
    Discard,
    Shuffle
}