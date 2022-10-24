using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Map;

public class GameManager : MonoBehaviour
{ 
    public bool shouldGenerateNewMap;
    public static GameManager instance;
    EncounterDifficulty nextCombatDiff;
    [SerializeField]
    NodeType nextReward;
    
    private void Awake() 
    {
        if(!instance)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void newGame()
    {
        shouldGenerateNewMap = true;
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
