using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardLibrary : MonoBehaviour
{
    [SerializeField] DeckManager_SO manager;
    public GameObject[] cardSlots;

    private void Start()
    {
        DisplayCard();
    }

    private void DisplayCard()
    {
        for (int i = 0; i < manager.getDeck.deckData.deckListData.Count; i++)
        {
            //TODO Display name, type, sprite
        }
    }
}
