// Written by
//  Javier Villegas
// Modified by
//  Henrik Hustoft

using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// SO containing all data necessary for all cards
/// </summary>
[System.Serializable]
public abstract class Card_SO : ScriptableObject
{
    public CardType type;
    public Texture Art;
    public Texture Background;
    public string cardName;
    [TextArea(6, 20)] public string description;
    [TextArea(6, 20)] public string effect;
    public abstract CardActionEnum[] getGlyphs();
}

