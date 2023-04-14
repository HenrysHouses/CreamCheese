using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class ItemDrop
{
    public float Weight;
    public float DropChance;
    [HideInInspector] public float UpperRange;
    public Object Item;

    public void updateDropChance(float lootPoolWeight)
    {
        if(Weight <= 0)
        {
            DropChance = 0;
            return;
        }

        DropChance = (Weight / lootPoolWeight * 100);
    }

    public ItemDrop Clone()
    {
        ItemDrop _Clone = new ItemDrop();

        _Clone.Weight = this.Weight;
        _Clone.DropChance = this.DropChance;
        _Clone.UpperRange = this.UpperRange;
        _Clone.Item = this.Item;

        return _Clone;
    }
}

public static class ItemRarity
{
    static readonly Dictionary<RarityType, float> RarityChances = 
        new Dictionary<RarityType, float>()
        {
            {RarityType.Common, 70.0f}, 
            {RarityType.Uncommon, 25.0f},
            {RarityType.Rare, 5.0f} 
        };

    static public float ChanceOf(RarityType rarity) 
    {
        float chance = 0;
        RarityChances.TryGetValue(rarity, out chance);
        return chance;
    }
}

public enum RarityType
{
    Common,
    Uncommon,
    Rare
}

public enum RewardType
{
    Card,
    Rune,
    Healing,
    Upgrade
}

/* 
# Item rarity documentation

All items are separated into common, uncommon, and rare rarities which has a constant base % chance to get.

Common : 70%
Uncommon : 25%
Rare : 5 %

Then each item has a weight value for how common it should be within the rarity category.

the final chance will be calculated by the individual item's weight % of sum of all the weight values

ex: 
Card_Attack_1 : "Weight"="1" "Drop Chance"="4%"
Card_Attack_2 : "Weight"="5" "Drop Chance"="20%"
Card_Attack_3 : "Weight"="5" "Drop Chance"="20%"
Card_Attack_4 : "Weight"="3" "Drop Chance"="12%"
Card_Attack_5 : "Weight"="7" "Drop Chance"="30%"
Card_Attack_6 : "Weight"="3" "Drop Chance"="12%"

Weight Sum: 24

Item drops are calculated by a random number between 0 and total weight.
then the item dropped will be selected by what range it lands on. the range is decided by the order of items in the item pool.
*/