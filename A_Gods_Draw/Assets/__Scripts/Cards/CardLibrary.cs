//charlie
//Henrik

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using HH.MultiSceneTools;

public class CardLibrary : MonoBehaviour
{
    [SerializeField] DeckManager_SO deckManager;
    [SerializeField] DeckList_SO deckList;
    public GameObject cardPrefab;
    public Transform[] cardSlots;
    int currPage;
    GameObject[] currDisplayedCards;

    public bool shouldDestroyACard;
    [SerializeField] Button backButton;
    [SerializeField] TextMeshPro PickCardText; 
    bool isAnimating;

    private void Start()
    {
        currDisplayedCards = new GameObject[cardSlots.Length];
        currPage = 0;
        DisplayCardPage(currPage);

        shouldDestroyACard = GameManager.instance.shouldDestroyCardInDeck;
        if(shouldDestroyACard)
        {
            backButton.interactable = false;
            PickCardText.gameObject.SetActive(true);
        }
    }

    void Update()
    {
        if(shouldDestroyACard)
        {
            tryDestroyCard();
        }
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
        if(DisplayCardPage(currPage+1))
            currPage++;
    }

    public void TurnBack()
    {
        if(DisplayCardPage(currPage-1))
            currPage--;
    }

    void tryDestroyCard()
    {
        int layer = 1 << 7;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if(!Physics.Raycast(ray, out RaycastHit hit, 10000, layer))
            return;            

        hit.transform.GetChild(0).gameObject.SetActive(true);
        hit.collider.GetComponentInChildren<DisableHighlight>().StayEnabled();

        if(!Input.GetMouseButtonDown(0))
            return;

        Card_SO _selectedCard = hit.collider.GetComponentInChildren<Card_Loader>().GetCardSO;
        deckManager.removeCardFromDeck(_selectedCard);
        Destroy(hit.collider.gameObject);
        shouldDestroyACard = false;

        StartCoroutine(destroyAnim());
    }

    IEnumerator destroyAnim()
    {
        isAnimating = true;
        yield return new WaitForEndOfFrame();
        isAnimating = false;
        GameManager.instance.DestroyedCardIsDone();
        MultiSceneLoader.loadCollection("Map", collectionLoadMode.Difference);
    }
}
