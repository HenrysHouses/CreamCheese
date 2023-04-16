using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct LootPoolRarity
{
    [SerializeField] private RarityChance<RarityType.Common> Common;
    [SerializeField] private RarityChance<RarityType.Uncommon> Uncommon;
    [SerializeField] private RarityChance<RarityType.Rare> Rare;
    [SerializeField] private RarityChance<RarityType.Legendary> Legendary;
    [SerializeField] private RarityChance<RarityType.Unique> Unique;

    public float ChanceOf(RarityType rarity) 
    {
        switch(rarity)
        {
            case RarityType.Common:
                return Common.Chance;

            case RarityType.Uncommon:
                return Uncommon.Chance;

            case RarityType.Rare:
                return Rare.Chance;

            case RarityType.Legendary:
                return Legendary.Chance;

            case RarityType.Unique:
                return Unique.Chance;

            default:
                return -1;
        }
    }

    public ItemPool_ScriptableObject RollRarity()
    {
        return null;
    }

    [System.Serializable]
    private struct RarityChance<T> where T : RarityType
    {
        /// <summary>Rarity type</summary>
        public T Rarity => new RarityType() as T;
        
        /// <summary>Chance in %</summary>
        [Range(0,100)] 
        public float Chance;
        
        public ItemPool_ScriptableObject ItemPool;
    }
}


// Could not use enum as a generic type. This was the first solution i could think of ¯\_(ツ)_/¯
public class RarityType
{
    public class Common : RarityType {}
    public class Uncommon : RarityType {}
    public class Rare : RarityType {}
    public class Legendary : RarityType {}
    public class Unique : RarityType {}
}