using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Loot Pool", menuName = "A_Gods_Draw/Loot Pool", order = 1)]
public class LootPool_ScriptableObject : ScriptableObject 
{
    public LootPoolRarity Rarities;
}
