using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarterDeckConfirmation : MonoBehaviour
{
    [SerializeField] ChooseStarterDeck chooseStarterDeck;

    void OnMouseDown()
    {
        chooseStarterDeck.shouldConfirmSelection = true;
    }
}
