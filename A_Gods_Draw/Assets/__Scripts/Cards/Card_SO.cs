using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(menuName = "ScriptableObjects/Card_Abs")]

public abstract class Card_SO : ScriptableObject
{
    public CardType type;
    public Sprite image;
    public string cardName;
    public string description;
    public int strengh;
}

