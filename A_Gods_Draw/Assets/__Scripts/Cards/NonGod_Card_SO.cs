using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Non-God card")]

public class NonGod_Card_SO : Card_SO
{
    public readonly Dictionary<CardActionEnum, int> cardActions;
    public readonly Sprite icon;
    public readonly GodActionEnum correspondingGod;
}
