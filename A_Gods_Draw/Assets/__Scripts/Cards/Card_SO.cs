// Written by
//  Javier Villegas
// Modified by
//  Henrik Hustoft

using UnityEngine;

/// <summary>
/// SO containing all data necessary for all cards
/// </summary>
[System.Serializable]
public abstract class Card_SO : ScriptableObject
{
    public CardType type;
    public Sprite image;
    public string cardName;
    [TextArea(6, 20)] public string description;
    [TextArea(6, 20)] public string effect;
}

