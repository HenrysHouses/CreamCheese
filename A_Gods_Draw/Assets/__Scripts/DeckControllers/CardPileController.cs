using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPileController : MonoBehaviour
{
    [SerializeField] DeckManager_SO deckManager;

    [SerializeField] bool UseLibraryOrDiscard;

    [SerializeField] GameObject[] Cards;

    bool isTransferring;
    
    void Start()
    {
        if(UseLibraryOrDiscard)
            deckManager.OnLibraryChange.AddListener(UpdatePile);
        else
            deckManager.OnDiscardChange.AddListener(UpdatePile);

        deckManager.OnShuffleDiscard.AddListener(StartTransfer);
        UpdatePile();
    }

    void UpdatePile()
    {
        if(isTransferring)
            return;

        float percent;
        if(UseLibraryOrDiscard)
            percent =  (float)deckManager.GetLibraryCount() / (float)deckManager.GetAllCurrentPlayingCards();
        else
            percent =  (float)deckManager.GetDiscardCount() / (float)deckManager.GetAllCurrentPlayingCards();
        float EnabledCardCount = (Cards.Length * percent);
        
        // * Rounding up code
        // float _factor = (Cards.Length * percent);
        // decimal EnabledCardCount = (decimal)_factor;

        // decimal factor = RoundFactor(0);
        // EnabledCardCount *= factor;
        // EnabledCardCount = System.Math.Ceiling(EnabledCardCount);
        // EnabledCardCount /= factor;

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
            time += Time.deltaTime;

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

    internal static decimal RoundFactor(int places)
    {
        decimal factor = 1m;

        if (places < 0)
        {
            places = -places;
            for (int i = 0; i < places; i++)
                factor /= 10m;
        }

        else
        {
            for (int i = 0; i < places; i++)
                factor *= 10m;
        }

        return factor;
    }

}
