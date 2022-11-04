using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(menuName = "ScriptableObjects/Card_Abs")]

[System.Serializable]
public abstract class Card_SO : ScriptableObject
{
    public CardType type;
    public Sprite image;
    public string cardName;
    [TextArea(6, 20)]
    public string description;
}

