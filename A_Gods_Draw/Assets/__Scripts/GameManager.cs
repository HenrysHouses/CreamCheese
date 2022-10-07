using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    EncounterDiffeculty nextCombatDiff;
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

    public EncounterDiffeculty nextCombatType{
        get { return nextCombatDiff; } 
        set { nextCombatDiff = value; }
    }  
}
