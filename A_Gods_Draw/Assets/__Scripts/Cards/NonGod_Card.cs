using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Card/Non-God card")]

public class NonGod_Card : Card_SO
{
    public string correspondingGod;
    public short baseStrength;

    public Sprite icon;

}
