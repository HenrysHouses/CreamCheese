using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HH.MultiSceneTools;

public class TurnOffBox : MonoBehaviour
{
    public GameObject toTurnOff;
    // Start is called before the first frame update
    void Start()
    {
        toTurnOff.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (MultiSceneLoader.currentlyLoaded.Title == "CameraTutorial")
        {
            toTurnOff.SetActive(true);
        }
        else  if (MultiSceneLoader.currentlyLoaded.Title == "CombatTutorial")
        {
            toTurnOff.SetActive(true);
        }
        else
        {
            toTurnOff.SetActive(false);
        }
    }
}
