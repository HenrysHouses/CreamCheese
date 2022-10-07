using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EncounterDiffeculty
{
    Easy,
    Medium,
    Hard,
    elites
}

public class EncounterGenerator : MonoBehaviour
{
    // Start is called before the first frame update
    public Encounter_SO[] LoadEncounters(EncounterDiffeculty ED)
    {

        return Resources.LoadAll<Encounter_SO>("Encounters/" + ED.ToString());
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
