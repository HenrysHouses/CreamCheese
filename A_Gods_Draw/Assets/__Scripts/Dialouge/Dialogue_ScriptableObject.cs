/* 
 * Written by 
 * Henrik
*/

using UnityEngine;
using FMODUnity;

/// <summary>ScriptableObject that contains all required data for a dialogue box</summary>
[CreateAssetMenu(menuName = "DialogueBox/Dialogue")]
public class Dialogue_ScriptableObject : ScriptableObject
{
    [SerializeField] public Dialogue dialogue;
}

/// <summary>Container for dialogue data</summary>
[System.Serializable]
public class Dialogue : IDialogue
{
    [field:SerializeField] 
    public override sentence[] pages {get; set;}
    [field:SerializeField]
    public override string TransformName {get; set;} = "MainCamera";
    [field:SerializeField]
    public override EventReference SFX {get; set;}
}

public abstract class IDialogue
{
    public abstract sentence[] pages {get; set;}
    public abstract string TransformName {get; set;}
    public abstract EventReference SFX {get; set;}
}

/// <summary>Container for all data in each page of a dialogue</summary>
[System.Serializable]
public class sentence
{
    [TextArea(5, 20)]
    public string text;
    public float speed = 0.05f;
}