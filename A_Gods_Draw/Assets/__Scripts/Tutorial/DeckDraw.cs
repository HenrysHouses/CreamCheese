using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeckDraw : MonoBehaviour
{
    public string[] tutorialText;
    bool[] tutorialChecks;
    public TMP_Text inputText;


    // Start is called before the first frame update
    void Start()
    {
        LoadTutorialDeck();
        inputText.text = tutorialText[0];


        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public DeckList_SO LoadTutorialDeck()
    {

     DeckList_SO loaded = Resources.Load<DeckList_SO>("Decklist/TutorialDeck.asset");
     return loaded;

    }

    public Encounter_SO LoadEncounter()
    {
        Encounter_SO loaded = Resources.Load<Encounter_SO>("Assets/Resources/Encounters/Tutorial/Tutorial Encounter.asset");
        return loaded;
    }
}
