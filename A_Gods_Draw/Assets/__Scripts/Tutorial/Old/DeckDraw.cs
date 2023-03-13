//! modified by charlie

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//! This is the tutorial level script
public class DeckDraw : MonoBehaviour
{
    public string[] tutorialTextWASD, cardText;
    public TMP_Text[] inputText;


    //? testing for things happening on specific things
    public GameObject[] popUps;
    private int popUpIndex;
    bool hasPressed;

    //? monster spawner stuff
    public GameObject enemy;

    // Start is called before the first frame update
    void Start()
    {
        //todo deactivate the enemies thats not supposed to be there
        //todo deactivate the cards spawning

        
    }

    // Update is called once per frame
    void Update()
    {
        CameraTutorial();
        CardTutorial();
    }

    /// <summary>
    /// takes the player through the different camera angles using WASD
    /// </summary>
    void CameraTutorial()
    {
        for (int i = 0; i < popUps.Length; i++)
        {
            if (i == popUpIndex)
            {
                popUps[i].SetActive(true);
            }
            //else
            //{
            //    popUps[i].SetActive(false);
            //}
        }
        if (popUpIndex == 0)
        {
            inputText[0].text = tutorialTextWASD[0];
            popUpIndex++;
        }

        //! all the events after one another
        if (popUpIndex == 1)
        {
            if (Input.GetMouseButtonDown(0))
            {
                inputText[0].text = tutorialTextWASD[1];
                popUpIndex++;
            }
        }
        else if (popUpIndex == 2)
        {
            if (Input.GetMouseButtonDown(0))
            {
                inputText[0].text = tutorialTextWASD[2];
                popUpIndex++;
            }
        }
        else if (popUpIndex == 3) //shows S image and text 4
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                inputText[0].text = tutorialTextWASD[3];
                popUpIndex++;
            }
        }
        else if (popUpIndex == 4) //shows text 5 and press s again
        {
            if (Input.GetKeyDown(KeyCode.S) && !hasPressed)
            {
                inputText[0].text = tutorialTextWASD[4];
                popUpIndex++;
                hasPressed = true;
            }

        }
        else if (popUpIndex == 5) // shows A image and text 6
        {
            if (Input.GetKeyDown(KeyCode.S) && hasPressed)
            {
                inputText[0].text = tutorialTextWASD[5];
                hasPressed = false;
                popUpIndex++;
            }
        }
        else if (popUpIndex == 6) // shows D image and text 7
        {
            if (Input.GetKeyDown(KeyCode.A) && !hasPressed)
            {
                inputText[0].text = tutorialTextWASD[6];
                hasPressed = true;
                popUpIndex++;
            }
        }
        else if (popUpIndex == 7) // shows text 8
        {
            if (Input.GetKeyDown(KeyCode.D) && hasPressed)
            {
                inputText[0].text = tutorialTextWASD[7];
                hasPressed = false;
                popUpIndex++;
            }
        }
        else if (popUpIndex == 8) //shows text 9
        {
            if (Input.GetKeyDown(KeyCode.D) && !hasPressed)
            {
                inputText[0].text = tutorialTextWASD[8];
                hasPressed = true;
                popUpIndex++;
            }

        }
        else if (popUpIndex == 9) // show text 10
        {
            if (Input.GetKeyDown(KeyCode.A) && hasPressed)
            {
                inputText[0].text = tutorialTextWASD[9];
                hasPressed = false;
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
        //! all the events in order
        if(popUpIndex == 10) //text 0 of cardtext
        {
            if (Input.GetMouseButtonDown(0))
            {
                LoadEncounter();
                //Instantiate(enemy, new Vector3(0,0.8f,0), Quaternion.identity);
                inputText[0].text = cardText[0];
                popUpIndex++;
            }
        }
        else if (popUpIndex == 11) //text 1 of cardtext
        {
            if (Input.GetMouseButtonDown(0))
            {
                LoadTutorialDeck();
                popUpIndex++;
            }
        }
        else if (popUpIndex == 12) //text 2 of cardtext
        {
            if (Input.GetMouseButtonDown(0))
            {
                inputText[0].text = cardText[1];
                popUpIndex++;
            }
        }
        //todo 1 enemy spawns, text msg saying use THIS Attack card on the enemy
        else if (popUpIndex == 13) //text 3 of cardtext
        {
            if (Input.GetMouseButtonDown(0))
            {
                popUpIndex++;
            }
        }
        else if (popUpIndex == 14) //text 3 of cardtext
        {
            if (Input.GetMouseButtonDown(0))
            {
                popUpIndex++;
            }
        }
        else if (popUpIndex == 15) //text 3 of cardtext
        {
            if (Input.GetMouseButtonDown(0))
            {
                popUpIndex++;
            }
        }
        else if (popUpIndex == 16) //text 3 of cardtext
        {
            if (Input.GetMouseButtonDown(0))
            {
                popUpIndex++;
            }
        }
        else if (popUpIndex == 17) //text 3 of cardtext
        {
            if (Input.GetMouseButtonDown(0))
            {
                popUpIndex++;
            }
        }
        else if (popUpIndex == 18) //text 3 of cardtext
        {
            if (Input.GetMouseButtonDown(0))
            {
                popUpIndex++;
            }
        }
        else if (popUpIndex == 19) //text 3 of cardtext
        {
            if (Input.GetMouseButtonDown(0))
            {
                popUpIndex++;
            }
        }
        else if (popUpIndex == 20) //text 3 of cardtext
        {
            if (Input.GetMouseButtonDown(0))
            {
                popUpIndex++;
            }
        }
        else if (popUpIndex == 21) //text 3 of cardtext
        {
            if (Input.GetMouseButtonDown(0))
            {
                popUpIndex++;
            }
        }
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
