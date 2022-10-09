using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Map;

public class GameManager : MonoBehaviour
{ 
    public bool shouldGenerateNewMap;
    public static GameManager instance;
    EncounterDiffeculty nextCombatDiff;
    
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

    public EncounterDiffeculty nextCombatType{
        get { return nextCombatDiff; } 
        set { nextCombatDiff = value; }
    }  
}
