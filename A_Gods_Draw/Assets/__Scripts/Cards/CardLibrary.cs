using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardLibrary : MonoBehaviour
{
    [SerializeField] DeckManager_SO manager;
    [SerializeField] DeckList_SO deckList;
    public GameObject cardPrefab;
    public Transform[] cardSlots;
    bool isCreated;

    private void Start()
    {
        isCreated = false;
    }

    private void Update()
    {
        DisplayCard();
    }

    private void DisplayCard()
    {

        if (!isCreated)
        {
            for (int i = 0; i < manager.getDeck.deckData.deckListData.Count; i++)
            {
                GameObject newObject = Instantiate(cardPrefab);
                newObject.transform.SetParent(cardSlots[i]);

                isCreated = true;
                Debug.Log("cards library");
            }

        }
    }

    public void TurnForward()
    {

    }

    public void TurnBack()
    {

    }
}
