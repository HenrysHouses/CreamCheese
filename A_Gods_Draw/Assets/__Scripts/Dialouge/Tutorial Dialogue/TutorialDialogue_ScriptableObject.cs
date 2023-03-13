using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DialogueBox/Tutorial Dialogue")]
public class TutorialDialogue_ScriptableObject : ScriptableObject
{
    [SerializeField] public TutorialDialogue dialogue;

    public TutorialDialogue_ScriptableObject Clone()
    {
        return this.MemberwiseClone() as TutorialDialogue_ScriptableObject;
    }
}