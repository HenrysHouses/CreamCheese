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
public class Dialogue
{
    public sentence[] pages;
    public string TransformName = "MainCamera";
    public EventReference SFX;
}

/// <summary>Container for all data in each page of a dialogue</summary>
[System.Serializable]
public struct sentence
{
    [TextArea(5, 20)]
    public string text;
    public float speed;
}