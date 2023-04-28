/*
 * Written by:
 * Henrik
 * 
 */

using UnityEngine;
using Map;
using FMOD.Studio;
using FMODUnity;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    // [SerializeField] DeckManager_SO deckManager;
    [SerializeField] public PlayerTracker PlayerTracker;

    public bool shouldGenerateNewMap;
    public bool shouldDestroyCardInDeck {get; private set;}
    private List<SceneIntensityEffect> intensityEffects;
    private float effectIntensity;
    public float EffectIntensity
    {
        get
        {
            return effectIntensity;
        }

        set
        {
            effectIntensity = value;
            UpdateSceneEffects();
        }
    }
    public static GameManager instance;
    EncounterDifficulty nextCombatDiff;
    [SerializeField]
    NodeType nextReward;

    [SerializeField] EventDescription eventDescription;

    static public int timesDefeatedBoss = 0;
    public bool PauseMenuIsOpen;


    private void Awake()
    {
        DeckList_SO.setPlayerDeck();

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

        intensityEffects = new List<SceneIntensityEffect>();
    }

    private void Start()
    {
        loadGameData();
    }

    private void UpdateSceneEffects()
    {

        foreach (SceneIntensityEffect _intensityEffect in intensityEffects)
        {

            _intensityEffect.UpdateIntensity(effectIntensity);
            
        }

    }

    public void AddSceneIntensityEffect(SceneIntensityEffect _intensityEffect)
    {

        intensityEffects.Add(_intensityEffect);

    }

    public void RemoveSceneIntensityEffect(SceneIntensityEffect _intensityEffect)
    {

        intensityEffects.Remove(_intensityEffect);

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
        PlayerTracker.CurrentRunes.Clear();
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

    public void DestroyCardFromDeck()
    {
        shouldDestroyCardInDeck = true;
    }

    public void DestroyedCardIsDone()
    {
        shouldDestroyCardInDeck = false;
    }
}
