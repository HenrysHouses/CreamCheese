using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(menuName = "Enemies/Monster")]
public class MonsterScriptableObject : ScriptableObject
{

    [SerializeField]
    public Attributes MonsterAttributes;

}

[System.Serializable]
public class Attributes
{

    public int MaxHealth;

}

[System.Serializable]
public class MonsterIntents
{



}