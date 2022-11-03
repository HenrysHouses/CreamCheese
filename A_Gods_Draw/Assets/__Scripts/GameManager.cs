using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Map;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    DeckManager_SO deckManager;

    public bool shouldGenerateNewMap;
    public static GameManager instance;
    EncounterDifficulty nextCombatDiff;
    [SerializeField]
    NodeType nextReward;
    
    private void Awake() 
    {
        GameSaver.InitializeSaving();
        if(!instance)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        deckManager.LoadDeck();
    }

    public void newGame()
    {
        shouldGenerateNewMap = true;
        // PlayerPrefs.DeleteAll();
    }

    public EncounterDifficulty nextCombatType{
        get { return nextCombatDiff; } 
        set { nextCombatDiff = value; }
    }  

    public NodeType nextRewardType
    {
        get { return nextReward; }
        set { nextReward = value; }
    }
}
