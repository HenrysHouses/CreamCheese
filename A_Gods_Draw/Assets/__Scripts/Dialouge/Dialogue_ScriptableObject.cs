using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

[CreateAssetMenu(menuName = "DialogueBox/Dialogue")]
public class Dialogue_ScriptableObject : ScriptableObject
{
    public Dialogue dialogue;
}

[System.Serializable]
public class Dialogue
{
    [TextArea(5, 20)]
    public string[] text;
    public string TransformName = "Camera";
    public EventReference SFX;
}