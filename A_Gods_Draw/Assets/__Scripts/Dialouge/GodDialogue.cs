using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "DialogueBox/God Dialogue")]
public class GodDialogue : Dialogue_ScriptableObject
{
    public GodDialogueTrigger trigger;
    public bool GenericTrigger = true;
    [HideInInspector] public string DialogueName;
}

public enum GodDialogueTrigger
{
    EnterBossFight,
    BossKill,
    Hurt,
    Dying,
    Draw,
    Played,
}