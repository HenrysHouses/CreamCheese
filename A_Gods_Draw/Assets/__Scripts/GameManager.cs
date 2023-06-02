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
using HH.MultiSceneTools;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // [SerializeField] DeckManager_SO deckManager;
    [SerializeField] SceneCollection TitleScreen;
    [SerializeField] public PlayerTracker PlayerTracker;

    public bool shouldGenerateNewMap;
    public bool shouldDestroyCardInDeck {get; private set;}
    private List<SceneIntensityEffect> intensityEffects;
    private float effectIntensity;
    public void UpdateEffectIntensity(float _intensity, bool _updateFromDamaged)
    {

        effectIntensity = _intensity;
        UpdateSceneEffects(_updateFromDamaged);

    }
    public static GameManager instance;
    [SerializeField] EncounterDifficulty nextCombatDiff;
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
        PlayerTracker.LoadPlayerData();

        if(SceneManager.sceneCount == 1)
            MultiSceneLoader.loadCollection(TitleScreen, collectionLoadMode.Difference);
    }

    private void UpdateSceneEffects(bool _updateFromDamage = false)
    {

        foreach (SceneIntensityEffect _intensityEffect in intensityEffects)
        {

            _intensityEffect.UpdateIntensity(effectIntensity, _updateFromDamage);
            
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
        timesDefeatedBoss = PlayerTracker.PlayerData.TimesDefeatedBoss;
    }

    public void newGame()
    {
        shouldGenerateNewMap = true;
        PlayerTracker.resetHealth();
        PlayerTracker.CurrentRunes.Clear();
        CardQuantityContainer newSave = new CardQuantityContainer();
        GameSaver.SaveData(newSave);
        GameSaver.SavePlayerData(new PlayerDataContainer(0, 30, new RuneData[0]));
        PlayerTracker.LoadPlayerData();
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
