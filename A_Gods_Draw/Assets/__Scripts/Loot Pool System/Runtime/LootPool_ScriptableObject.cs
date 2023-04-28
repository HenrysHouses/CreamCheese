using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Loot Rarity", menuName = "A_Gods_Draw/Loot Rarity", order = 1)]
public class LootPool_ScriptableObject : ScriptableObject 
{
    public LootPoolRarity Rarities;

    private void OnValidate() {
        Rarities.updateRarityIndex();
    }

    public ItemPool_ScriptableObject Roll(out RarityType DroppedRarity)
    {
        ItemPool_ScriptableObject pool = Rarities.RollRarity(out RarityType drop);
        DroppedRarity = drop;
        return pool;
    } 
}
