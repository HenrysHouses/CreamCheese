using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Non-God card")]

public class NonGod_Card_SO : Card_SO
{
    public Sprite icon;
    public List<CardActionData> cardActions;
    public GodActionEnum correspondingGod;
}

[System.Serializable]
public struct CardActionData
{
    public CardActionEnum actionEnum;
    public int actionStrength;
}
