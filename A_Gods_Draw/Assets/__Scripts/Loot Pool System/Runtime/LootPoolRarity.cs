using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LootPoolRarity
{
    [SerializeField] private RarityChance Common = new RarityChance(RarityType.Common);
    [SerializeField] private RarityChance Uncommon = new RarityChance(RarityType.Uncommon);
    [SerializeField] private RarityChance Rare = new RarityChance(RarityType.Rare);
    [SerializeField] private RarityChance Legendary = new RarityChance(RarityType.Legendary);
    [SerializeField] private RarityChance Unique = new RarityChance(RarityType.Unique);

    [SerializeField] private RarityChance[] RarityIndex;

    public float GetChanceOf(RarityType rarity) 
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

    public ItemPool_ScriptableObject RollRarity(out RarityType RarityDropped)
    {
        float _roll = Random.Range(0f,100f);
        float chance = 0;

        for (int i = 0; i < RarityIndex.Length; i++)
        {
            if(RarityIndex[i].Chance + chance > _roll)
            {

                Debug.Log(RarityIndex[i].Chance + chance + "/"+ _roll);
                RarityDropped = RarityIndex[i].Rarity;
                return RarityIndex[i].ItemPool;
            }
            chance += RarityIndex[i].Chance;
        }
        RarityDropped = RarityType.None;
        return null;
    }

    public void updateRarityIndex()
    {
        RarityIndex = new RarityChance[]{Common, Uncommon, Rare, Legendary, Unique};
        List<RarityChance> Order = new List<RarityChance>(5);
        Order.Add(new RarityChance());
        Order.Add(new RarityChance());
        Order.Add(new RarityChance());
        Order.Add(new RarityChance());
        Order.Add(new RarityChance());

        for (int i = 0; i < 5; i++)
        {
            int index = 0;
            for (int j = 0; j < 5; j++)
            {
                if(RarityIndex[i].Rarity == RarityIndex[j].Rarity)
                    continue;

                if(RarityIndex[i].Chance > RarityIndex[j].Chance)
                    index++;
            }
            Order[index] = RarityIndex[i];
        }

        Order.Reverse();
        RarityIndex = Order.ToArray();
    }

    [System.Serializable]
    private struct RarityChance
    {
        public RarityChance(RarityType type)
        {
            Rarity = type;
            Chance = 0;
            ItemPool = null;
        }

        /// <summary>Rarity type</summary>
        public RarityType Rarity;
        
        /// <summary>Chance in %</summary>
        [Range(0,100)] 
        public float Chance;
        
        public ItemPool_ScriptableObject ItemPool;
    }
}


// Could not use enum as a generic type. This was the first solution i could think of ¯\_(ツ)_/¯
public enum RarityType
{
    Common,
    Uncommon,
    Rare,
    Legendary,
    Unique,
    None
}