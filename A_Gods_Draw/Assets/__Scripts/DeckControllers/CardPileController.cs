/* 
 * Written by 
 * Henrik
*/

using System.Collections;
using UnityEngine;

/// <summary>
/// Controls the amount of cards in a pile for visual feedback
/// </summary>
public class CardPileController : MonoBehaviour
{
    [SerializeField] DeckController deckController;

    [SerializeField] bool UseLibraryOrDiscard;

    [SerializeField] GameObject[] Cards;
    [SerializeField] float speed = 1;

    bool isTransferring;
    
    void Start()
    {
        if(UseLibraryOrDiscard)
            deckController.OnLibraryChange.AddListener(UpdatePile);
        else
            deckController.OnDiscardChange.AddListener(UpdatePile);

        deckController.OnShuffleDiscard.AddListener(StartTransfer);
        UpdatePile();
    }

    void UpdatePile()
    {
        if(isTransferring)
            return;

        float percent;
        if(UseLibraryOrDiscard)
            percent =  (float)deckController.libraryCount / (float)deckController.cardsCount;
        else
            percent =  (float)deckController.discardCount / (float)deckController.cardsCount;
        float EnabledCardCount = (Cards.Length * percent);

        for (int i = 0; i < Cards.Length; i++)
        {
            if(i < EnabledCardCount)
                Cards[i].SetActive(true);
            else
                Cards[i].SetActive(false);
        }
    }

    void StartTransfer()  => StartCoroutine(TransferDiscardToLibrary(2));
    IEnumerator TransferDiscardToLibrary(float duration)
    {
        isTransferring = true;
        float time = 0;
        int NumOfCards = Cards.Length;
        int n = 0;

        if(!UseLibraryOrDiscard)
        {
            NumOfCards = GetNumOfEnabledCards();;
            n = NumOfCards;
        }

        while(time < duration)
        {
            time += Time.deltaTime * speed;

            if(UseLibraryOrDiscard)
            {
                if(time > (duration/NumOfCards)*(1+n))
                {
                    Cards[n].SetActive(true);
                    n++;
                }
            }
            else
            {
                if(time > (duration/NumOfCards)*(6-n))
                {
                    Cards[n-1].SetActive(false);
                    n--;
                }
            }

            yield return new WaitForEndOfFrame();
        }
        isTransferring = false;
    }

    int GetNumOfEnabledCards()
    {
        int n = 0;
        foreach (var obj in Cards)
        {
            if(obj.activeSelf)
                n++;
        }
        return n;
    }
}
