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

        if(targetReward == NodeType.Elite)
        {
            rune DroppedRune = _RarityDrop.getDroppedItem<rune>(player.CurrentRunes.ToArray() as Object[]);
            Card_SO DroppedCard;

            if(DroppedRune == null)
            {
                DroppedCard = CardDrop(_RarityDrop, true);
                spawnCard();
            }
            else
                spawnRune();
        }
        else if(targetReward == NodeType.RuneReward)
        {
            rune DroppedRune = _RarityDrop.getDroppedItem<rune>(player.CurrentRunes.ToArray() as Object[]);
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
            return ItemPool.getDroppedItem<Card_SO>(player.CurrentDeck.deckData.getCurrentCards());           
        else
            return ItemPool.getDroppedItem<Card_SO>();
    }

    Card_SO CardDrop(ItemPool_ScriptableObject ItemPool, bool UniqueDropsOverride = false)
    {
        if(UniqueDropsOverride)
            return ItemPool.getDroppedItem<Card_SO>(player.CurrentDeck.deckData.getCurrentCards());           
        else
            return ItemPool.getDroppedItem<Card_SO>();
    }

    void spawnRune()
    {
        GameObject RuneObj = Instantiate(RunePrefab);
    }

    void spawnCard()
    {
        GameObject RuneObj = Instantiate(CardPrefab);
    }
}


[System.Serializable]
public struct LootPoolTypes
{
    public NodeType RewardType;
    public LootPool_ScriptableObject LootPool;
}
