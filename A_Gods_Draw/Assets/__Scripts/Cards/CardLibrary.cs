using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardLibrary : MonoBehaviour
{
    [SerializeField] DeckList_SO deckList;
    public GameObject cardPrefab;
    public Transform[] cardSlots;
    int currPage;
    GameObject[] currDisplayedCards;

    public bool shouldDestroyACard;
    [SerializeField] Button backButton;

    private void Start()
    {
        currDisplayedCards = new GameObject[cardSlots.Length];
        currPage = 0;
        DisplayCardPage(currPage);

        if(shouldDestroyACard)
            backButton.interactable = false;
    }

    private bool DisplayCardPage(int page)
    {
        List<Card_SO> deck = deckList.deckData.deckListData;
        int DisplayOffset = cardSlots.Length * page;

        if(DisplayOffset > deck.Count || page < 0)
        {
            Debug.Log("You are at the last page");
            return false;
        }
        else
            clearPage();

        Debug.Log("displaying library page: " + page);
        for (int i = 0; i < cardSlots.Length; i++)
        {   
            if(DisplayOffset+i >= deck.Count)
                return true;

            GameObject spawnCard = Instantiate(cardPrefab);
            Card_Loader _Loader = spawnCard.GetComponentInChildren<Card_Loader>();
            _Loader.Set(deck[DisplayOffset+i]);

            spawnCard.transform.SetParent(cardSlots[i], false);
            currDisplayedCards[i] = spawnCard;
        }
        return true;
    }

    void clearPage()
    {
        foreach (var card in currDisplayedCards)
        {
            Destroy(card);
        }
    }

    public void TurnForward()
    {
        if(DisplayCardPage(currPage++))
            currPage++;
    }

    public void TurnBack()
    {
        if(DisplayCardPage(currPage--))
            currPage--;
    }
}
