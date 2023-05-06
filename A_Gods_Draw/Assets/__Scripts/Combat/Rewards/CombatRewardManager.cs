using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Map;
using HH.MultiSceneTools;

public class CombatRewardManager : MonoBehaviour
{
    [SerializeField] LootPoolTypes[] LootPools;
    [SerializeField] PlayerTracker player;
    [SerializeField] GameObject RunePrefab;
    [SerializeField] GameObject CardPrefab;
    [SerializeField] Transform[] SpawnPosition;
    [SerializeField] ChooseCardReward CardRewardController;
    [SerializeField] ChooseRuneReward RuneRewardController;
    [SerializeField] GameObject DeckGameObject;

    NodeType targetReward;

    // Start is called before the first frame update
    void Start()
    {
        CameraMovement.instance.SetCameraView(CameraView.CardReward);
        targetReward = GameManager.instance.nextRewardType;
        rollItem();
    }

    void rollItem()
    {
        LootPoolTypes targetLootPool = findLootPool(targetReward);
        Debug.Log(targetReward);

        if(targetLootPool == null)
        {
            MultiSceneLoader.loadCollection("Map", collectionLoadMode.Difference);
            return;
        }

        if(targetLootPool.LootPool == null)
            return;

        ItemPool_ScriptableObject _RarityDrop = targetLootPool.LootPool.Roll();

        switch(targetReward)
        {
            case NodeType.Elite:
                EliteReward(_RarityDrop);
                break;

            case NodeType.Boss:
                Card_SO DroppedCard = RollCard(_RarityDrop); // roll card
                spawnCard(DroppedCard, 1);
                break;

            case NodeType.RuneReward:
                Object DroppedRune = _RarityDrop.getDroppedItem(player.CurrentRunes.ToArray() as Object[]);
                RuneType targetRune = getRuneType(DroppedRune.name);
                spawnRune(targetRune);
                break;

            default:
            // case NodeType.RandomReward:
            // case NodeType.AttackReward:
            // case NodeType.DefenceReward:
            // case NodeType.BuffReward:
            // case NodeType.GodReward:
                DefaultCardReward(targetLootPool.LootPool);
                break;
        }
    }

    LootPoolTypes findLootPool(NodeType Type, int unlockIndex = 0)
    {
        int unlock = 0;
        for (int i = 0; i < LootPools.Length; i++)
        {
            if(LootPools[i].RewardType == Type)
            {
                if(unlock == unlockIndex)
                    return LootPools[i];
                else
                    unlock++;
            }
        }
        return null;
    }

    void DefaultCardReward(LootPool_ScriptableObject lootPool)
    {
        ItemPool_ScriptableObject itemPool = lootPool.Roll(); // roll rarity
        Card_SO DroppedCard0 = RollCard(itemPool); // roll card
        spawnCard(DroppedCard0, 0);

        itemPool = lootPool.Roll(); // roll rarity
        Card_SO DroppedCard1 = RollCard(itemPool); // roll card
        spawnCard(DroppedCard1, 1);

        itemPool = lootPool.Roll(); // roll rarity
        Card_SO DroppedCard2 = RollCard(itemPool); // roll card
        spawnCard(DroppedCard2, 2);
    }

    void EliteReward(ItemPool_ScriptableObject itemPool)
    {
        if(player.CurrentRunes.Count >= 2)
                Debug.Log(player.CurrentRunes[0] + " - " + player.CurrentRunes[1]);

        Object DroppedRune = null;
        DroppedRune = itemPool.getDroppedItem(player.CurrentRunes.ToArray() as Object[], true);

        if(DroppedRune)
        {
            RuneType targetRune = getRuneType(DroppedRune.name);
            spawnRune(targetRune);
        }
        else
        {
            LootPoolTypes targetLootPool = findLootPool(targetReward, 1);
            ItemPool_ScriptableObject _RarityDrop = targetLootPool.LootPool.Roll();

            Card_SO DroppedCard = RollCard(_RarityDrop, true);
            spawnCard(DroppedCard, 1);
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

    Card_SO RollCard(ItemPool_ScriptableObject ItemPool, bool UniqueDropsOverride = false)
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
        RuneRewardController.gameObject.SetActive(true);
    }

    void spawnCard(Card_SO card, int position)
    {
        Debug.Log("spawn card: " + card.name);
        GameObject CardObj = Instantiate(CardPrefab);
        Card_Loader loader = CardObj.GetComponentInChildren<Card_Loader>();
        
        CardPlayData NewCardData = new CardPlayData(card);
        loader.Set(NewCardData);
        CardObj.GetComponentInChildren<Card_InspectingPopup>().SetDescriptions(card as ActionCard_ScriptableObject, CardObj.gameObject);

        CardObj.transform.SetParent(SpawnPosition[position]);
        CardObj.transform.localPosition = Vector3.zero;
        CardObj.transform.localRotation = Quaternion.identity;

        CardObj.GetComponentInParent<CardRewardOption>().AddToDeck = card;
        DeckGameObject.SetActive(true);
        CardRewardController.gameObject.SetActive(true);
    }
}


[System.Serializable]
public class LootPoolTypes
{
    public NodeType RewardType;
    public LootPool_ScriptableObject LootPool;
}
