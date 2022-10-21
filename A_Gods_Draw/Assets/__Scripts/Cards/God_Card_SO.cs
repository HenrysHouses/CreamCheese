using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/God card")]

public class God_Card_SO : Card_SO
{
    [HideInInspector]
    public readonly new CardType type = CardType.God;
    public readonly int health;
    public readonly GodActionEnum godAction;
}