using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardLibrary : MonoBehaviour
{
    [SerializeField] DeckManager_SO manager;
    public GameObject[] cardSlots;

    private void Update()
    {
        DisplayCard();
    }

    private void DisplayCard()
    {
        for (int i = 0; i < manager.getDeck.deckData.deckListData.Count; i++)
        {
            Instantiate(manager.getDeck.deckData.deckListData[i]);
        }
    }
}
