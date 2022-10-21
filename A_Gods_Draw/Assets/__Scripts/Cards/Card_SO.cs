using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(menuName = "ScriptableObjects/Card_Abs")]

public abstract class Card_SO : ScriptableObject
{
    public readonly CardType type;
    public readonly Sprite image;
    public readonly string cardName;
    public readonly string description;
    public readonly int strengh;
}

