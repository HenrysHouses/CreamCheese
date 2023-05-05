using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Map;
using System.Reflection;

public class CombatRewardManager : MonoBehaviour
{
    [SerializeField] LootPoolTypes[] LootPools;
    [SerializeField] PlayerTracker player;
    [SerializeField] GameObject RunePrefab;
    [SerializeField] GameObject CardPrefab;
    [SerializeField] Transform[] SpawnPosition;
    [SerializeField] ChooseCardReward CardRewardController;
    [SerializeField] ChooseRuneReward RuneRewardController;

    // Start is called before the first frame update
    void Start()
    {
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

            if(player.CurrentRunes.Count >= 2)
                Debug.Log(player.CurrentRunes[0] + " - " + player.CurrentRunes[1]);

            Object DroppedRune = null;
            DroppedRune = _RarityDrop.getDroppedItem(player.CurrentRunes.ToArray() as Object[], true);

            if(DroppedRune)
            {
                RuneType targetRune = getRuneType(DroppedRune.name);
                spawnRune(targetRune);
            }
            else
            {
                Debug.LogError("No runes");
                // Card_SO DroppedCard = CardDrop(_RarityDrop, true);
                // spawnCard();
            }
        }
        else if(targetReward == NodeType.RuneReward)
        {
            Object DroppedRune = _RarityDrop.getDroppedItem(player.CurrentRunes.ToArray() as Object[]);
            RuneType targetRune = getRuneType(DroppedRune.ToString());
            spawnRune(targetRune);
        }
        else
        {
            Card_SO DroppedCard = CardDrop(_RarityDrop, _rarityType);
            spawnCard();
        }
    }

    public RuneType getRuneType(string script)
    {
        if(script.Contains("WealthRune"))    
            return RuneType.FeWealth;
        
        if(script.Contains("ChaosRune"))    
            return RuneType.TursChaos;

        if(script.Contains("StrengthRune"))    
            return RuneType.UrrStrength;

        return RuneType.None;
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

    void spawnRune(RuneType rune)
    {
        Debug.Log("spawn rune");
        GameObject RuneObj = Instantiate(RunePrefab);
        RuneObj.GetComponent<RuneSelector>().set(rune);

        RuneObj.transform.SetParent(SpawnPosition[1]);
        RuneObj.transform.localPosition = Vector3.zero;
        RuneObj.transform.localRotation = Quaternion.identity;

        RuneObj.GetComponent<CardRewardConfirmation>().chooseRuneReward = RuneRewardController;
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
