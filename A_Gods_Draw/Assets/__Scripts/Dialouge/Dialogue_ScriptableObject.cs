using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

[CreateAssetMenu(menuName = "DialogueBox/Dialogue")]
public class Dialogue_ScriptableObject : ScriptableObject
{
    [SerializeField] public Dialogue dialogue;
}

[System.Serializable]
public class Dialogue
{
    public sentence[] pages;
    public string TransformName = "MainCamera";
    public EventReference SFX;
}

[System.Serializable]
public struct sentence
{
    [TextArea(5, 20)]
    public string text;
    public float speed;
}