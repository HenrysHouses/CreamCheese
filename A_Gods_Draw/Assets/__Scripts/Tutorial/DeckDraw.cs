//! modified by charlie

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//! This is the tutorial level script
public class DeckDraw : MonoBehaviour
{
    public string[] tutorialTextWASD, cardText;
    bool[] tutorialChecks;
    public TMP_Text inputText;


    //? testing for things happening on specific things
    public GameObject[] popUpsWASD, popUpsCard;
    private int popUpIndex;
    bool hasPressed;

    //? monster spawner stuff
    public GameObject enemy, panel;

    // Start is called before the first frame update
    void Start()
    {
        //todo deactivate the enemies thats not supposed to be there

        LoadTutorialDeck();
        inputText.text = tutorialTextWASD[0];
    }

    // Update is called once per frame
    void Update()
    {
        CameraTutorial();

        //CardTutorial();
    }

    /// <summary>
    /// takes the player through the different camera angles using WASD
    /// </summary>
    void CameraTutorial()
    {
        for (int i = 0; i < popUpsWASD.Length; i++)
        {
            if (i == popUpIndex)
            {
                popUpsWASD[i].SetActive(true);
            }
            else
            {
                popUpsWASD[i].SetActive(false);
            }
        }

        //! all the events after one another
        if (popUpIndex == 0) //shows first text
        {
            if (Input.GetMouseButtonDown(0))
            {
                inputText.text = tutorialTextWASD[1];
                popUpIndex++;
            }
        }
        else if (popUpIndex == 1) //shows W image
        {
            if (Input.GetMouseButtonDown(0))
            {
                panel.SetActive(false);
                inputText.text = tutorialTextWASD[2];
                popUpsWASD[2].SetActive(true);
                popUpIndex++;
            }
        }
        else if (popUpIndex == 2) //shows S image
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                inputText.text = tutorialTextWASD[3];
                popUpsWASD[3].SetActive(true);
                popUpIndex++;
            }
        }
        else if (popUpIndex == 3) //shows text 4
        {
            if (Input.GetKeyDown(KeyCode.S) && !hasPressed)
            {
                panel.SetActive(true);
                inputText.text = tutorialTextWASD[4];
                popUpIndex++;
                hasPressed = true;
            }

        }
        else if (popUpIndex == 4) // shows A image
        {
            if (Input.GetKeyDown(KeyCode.S) && hasPressed)
            {
                inputText.text = tutorialTextWASD[5];
                popUpsWASD[5].SetActive(true);
                hasPressed = false;
                popUpIndex++;
            }
        }
        else if (popUpIndex == 5) // shows D image
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                inputText.text = tutorialTextWASD[6];
                popUpsWASD[6].SetActive(true);
                popUpIndex++;
            }
        }
        else if (popUpIndex == 6) // shows text 3
        {
            if (Input.GetKeyDown(KeyCode.D) && !hasPressed)
            {
                panel.SetActive(true);
                inputText.enabled = true;
                inputText.text = tutorialTextWASD[7];
                popUpIndex++;
                hasPressed = true;
            }
        }
        else if (popUpIndex == 7) //shows text 4
        {
            if (Input.GetKeyDown(KeyCode.D) && hasPressed)
            {
                inputText.text = tutorialTextWASD[8];
                popUpIndex++;
                hasPressed = false;
            }

        }
        else if (popUpIndex == 8) // show text 5
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                panel.SetActive(true);
                inputText.text = tutorialTextWASD[9];
                popUpIndex++;
            }
        }
    }


    /// <summary>
    /// teaches the player what the different cards do and how the enemies work
    /// 1. Attack card
    /// 2.enemies wanting to attack you so Shield card
    /// 3. Buff card then Attack card
    /// 4.Ending turn and going to map
    /// 5. God card
    /// 6. win encounter get sent to a special map with a rune node
    /// </summary>
    void CardTutorial()
    {
        for (int i = 0; i < popUpsCard.Length; i++)
        {
            if (i == popUpIndex)
            {
                popUpsCard[i].SetActive(true);
            }
            else
            {
                popUpsCard[i].SetActive(false);
            }
        }

        //! all the events in order
        if(popUpIndex == 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                inputText.text = cardText[0];
                popUpsCard[0].SetActive(true);
                popUpIndex++;
            }
        }
        //todo 1 enemy spawns, text msg saying use THIS Attack card on the enemy
        //todo get sent to map to choose new encounter
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
