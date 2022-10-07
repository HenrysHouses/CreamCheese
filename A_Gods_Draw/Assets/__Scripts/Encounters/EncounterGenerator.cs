using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EncounterDiffeculty
{
    Easy,
    Medium,
    Hard,
    elites,
    Boss
}

public class EncounterGenerator : MonoBehaviour
{
    // Start is called before the first frame update
    public Encounter_SO[] LoadNextEncounter()
    {
        return Resources.LoadAll<Encounter_SO>("Encounters/" + GameManager.instance.nextCombatType.ToString());
    }

    // Update is called once per frame
    void Update()
    {


        
    }
}
