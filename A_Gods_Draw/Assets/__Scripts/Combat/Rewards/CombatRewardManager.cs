using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Map;

public class CombatRewardManager : MonoBehaviour
{
    [SerializeField] LootPoolTypes[] LootPools;
    [SerializeField] PlayerTracker player;
    [SerializeField] GameObject RunePrefab;
    [SerializeField] GameObject CardPrefab;

    // Start is called before the first frame update
    void Start()
    {
        // player.addRune(new ChaosRune(1, RuneState.Active));
        // player.addRune(new WealthRune(1, RuneState.Active));
        // player.addRune(new StrengthRune(1, RuneState.Active));
        rollItem();
    }

    void rollItem()
    {
        LootPoolTypes targetLootPool = new LootPoolTypes();
        NodeType targetReward = GameManager.instance.nextRewardType;

        for (int i = 0; i < LootPools.Length; i++)
        {
            if(LootPools[i].RewardType == targetReward)
            {
                targetLootPool = LootPools[i];
                break;
            }
        }

        if(targetLootPool.LootPool == null)
            return;

        ItemPool_ScriptableObject _RarityDrop = targetLootPool.LootPool.Roll(out RarityType _rarityType);
        Debug.Log("Dropped " + _rarityType + ": " + _RarityDrop);

        if(targetReward == NodeType.Elite)
        {
            var DroppedRune = _RarityDrop.getDroppedItem(player.CurrentRunes.ToArray() as Object[], true);

            // Debug.Log(DroppedRune.ToString());
            Debug.Log(DroppedRune);

            if(DroppedRune)
            {
                spawnRune();
            }
            else
            {
                // Card_SO DroppedCard = CardDrop(_RarityDrop, true);
                // spawnCard();
            }
        }
        else if(targetReward == NodeType.RuneReward)
        {
            Object DroppedRune = _RarityDrop.getDroppedItem(player.CurrentRunes.ToArray() as Object[]);
            spawnRune();
        }
        else
        {
            Card_SO DroppedCard = CardDrop(_RarityDrop, _rarityType);
            spawnCard();
        }
    }

    Card_SO CardDrop(ItemPool_ScriptableObject ItemPool, RarityType rarityType)
    {
        if(rarityType == RarityType.Unique)
            return ItemPool.getDroppedItem(player.CurrentDeck.deckData.getCurrentCards()) as Card_SO;           
        else
            return ItemPool.getDroppedItem(false) as Card_SO;
    }

    Card_SO CardDrop(ItemPool_ScriptableObject ItemPool, bool UniqueDropsOverride = false)
    {
        if(UniqueDropsOverride)
            return ItemPool.getDroppedItem(player.CurrentDeck.deckData.getCurrentCards()) as Card_SO;           
        else
            return ItemPool.getDroppedItem(false) as Card_SO;
    }

    void spawnRune()
    {
        Debug.Log("spawn rune");
        // GameObject RuneObj = Instantiate(RunePrefab);

    }

    void spawnCard()
    {
        Debug.Log("spawn card");
        // GameObject RuneObj = Instantiate(CardPrefab);
    }
}


[System.Serializable]
public struct LootPoolTypes
{
    public NodeType RewardType;
    public LootPool_ScriptableObject LootPool;
}
