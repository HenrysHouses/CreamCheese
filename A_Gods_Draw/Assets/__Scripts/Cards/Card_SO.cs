using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//[CreateAssetMenu(menuName = "ScriptableObjects/Card_Abs")]

public abstract class Card_SO : ScriptableObject
{
    public Sprite image;
    public string cardName;
    public string description;
    public virtual Card_Behaviour Init(GameObject a)
    {
        return null;
    }
}

