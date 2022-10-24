using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Non-God card")]

public class NonGod_Card_SO : Card_SO
{
    public Sprite icon;
    public List<CardActionEnum> cardActions;
    public List<int> actionStrengh;
    public GodActionEnum correspondingGod;
}
