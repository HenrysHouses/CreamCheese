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
    [SerializeField] float Delay = 0.7f;
    int currentEnabledCards;
    bool updating;

    void Start()
    {
        for (int i = 0; i < Cards.Length; i++)
        {
            if(Cards[i].activeSelf)
                currentEnabledCards++;
        }
    }

    void Update()
    {
        float library = deckController.libraryCount;
        float discard = deckController.discardCount;

        float totalCards = deckController.cardsCount;
        
        float percent;
        if(UseLibraryOrDiscard)
            percent =  library / totalCards;
        else
            percent =  discard / totalCards;
        float EnabledCardCount = (Cards.Length * percent);

        if(currentEnabledCards != EnabledCardCount)
        {
            StartCoroutine(updatePile(EnabledCardCount));
        }
    }

    IEnumerator updatePile(float EnabledCardCount)
    {
        updating = true;

        for (int i = 0; i < Cards.Length; i++)
        {
            if(i < EnabledCardCount)
            {
                if(!Cards[i].activeSelf)
                {
                    Cards[i].SetActive(true);
                    currentEnabledCards++;
                }
            }
            else
            {
                if(Cards[i].activeSelf)
                {
                    Cards[i].SetActive(false);
                    currentEnabledCards--;
                }
            }
            yield return new WaitForSeconds(Delay);
        }
        updating = false;
    }
}