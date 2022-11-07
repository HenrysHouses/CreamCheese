using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CardSearch
{
    public static List<Card_SO> Search<T>(string[] SearchOptions = null) where T : Card_SO
    {
        List<Card_SO> outputResults = new List<Card_SO>();

        Card_SO[] allResults = Resources.LoadAll<Card_SO>("Cards");

        if(SearchOptions != null)
        {
            for (int j = 0; j < allResults.Length-1; j++)
            {
                for (int i = 0; i < SearchOptions.Length; i++)
                {
                    if(allResults[j] == null)
                    {
                        break;
                    }

                    if (allResults[j].cardName == SearchOptions[i])
                    {
                        outputResults.Add(allResults[j]);
                        break;
                    }

                    if (allResults[j].type.ToString() == SearchOptions[i])
                    {
                        outputResults.Add(allResults[j]);
                        break;
                    }
                    
                    NonGod_Card_SO card = allResults[j] as NonGod_Card_SO;

                    if(card)
                    {
                        if (card.correspondingGod.ToString() == SearchOptions[i])
                        {
                            outputResults.Add(allResults[j]);
                            break;
                        }
                    }                        
                }
            }
        }
        return outputResults as List<Card_SO>;
    }
}