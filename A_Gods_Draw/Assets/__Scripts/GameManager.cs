using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Map;
using FMOD.Studio;
using FMODUnity;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    DeckManager_SO deckManager;
    [SerializeField] PlayerTracker PlayerTracker;

    public bool shouldGenerateNewMap;
    public static GameManager instance;
    EncounterDifficulty nextCombatDiff;
    [SerializeField]
    NodeType nextReward;

    [SerializeField] EventDescription eventDescription;

    private void Awake()
    {

        GameSaver.InitializeSaving();
        if (!instance)
            instance = this;
        else
            Destroy(gameObject);

        RuntimeManager.LoadBank("Ambience", true);
        RuntimeManager.LoadBank("PlayerEffects", true);
        RuntimeManager.LoadBank("Master", true);
        RuntimeManager.LoadBank("CardEffects", true);
        RuntimeManager.LoadBank("MonsterEffects", true);
        RuntimeManager.LoadBank("GodEffects", true);
        //RuntimeManager.LoadBank("Master", true);
    }

    private void Start()
    {
        loadGameData();
    }

    private void loadGameData()
    {
        DeckListData deckList = GameSaver.LoadData();
        PlayerTracker.setDeck(deckList);
    }

    public void newGame()
    {
        shouldGenerateNewMap = true;
        PlayerTracker.resetHealth();
        CardQuantityContainer newSave = new CardQuantityContainer();
        GameSaver.SaveData(newSave);
        loadGameData();
    }

    public EncounterDifficulty nextCombatType
    {
        get { return nextCombatDiff; }
        set { nextCombatDiff = value; }
    }

    public NodeType nextRewardType
    {
        get { return nextReward; }
        set { nextReward = value; }
    }
}
