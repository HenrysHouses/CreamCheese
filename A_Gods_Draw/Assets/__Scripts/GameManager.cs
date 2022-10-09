using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Map;

public class GameManager : MonoBehaviour
{ public bool startedNewGame;
    public static GameManager instance;
    EncounterDiffeculty nextCombatDiff;
    public Map_Manager MM;
    
    private void Awake() 
    {
        if(!instance)
            instance = this;
        else
            Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        
    
    }

    public void newGame()
    {
        MM.GenerateNewMap();
        

    }

    public EncounterDiffeculty nextCombatType{
        get { return nextCombatDiff; } 
        set { nextCombatDiff = value; }
    }  
}
