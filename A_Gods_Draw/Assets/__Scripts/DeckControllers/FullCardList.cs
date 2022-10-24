using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CardSearch
{
    public static List<Card_SO> Search<T>(string[] SearchOptions = null) where T : Card_SO
    {
        List<Card_SO> Results = new List<Card_SO>();

        Card_SO[] currResults = Resources.LoadAll<Card_SO>("Cards");

        for (int i = 0; i < currResults.Length; i++)
        {
            if(currResults[i] is T)
                Results.Add(currResults[i]);
        }

        if(SearchOptions != null)
        {
            for (int i = 0; i < SearchOptions.Length; i++)
            {
                for (int j = 0; j < Results.Count; j++)
                {
                    bool shouldBeRemoved = true;

                    if (!Results[j].name.Equals(SearchOptions[i]))
                        shouldBeRemoved = false;

                    NonGod_Card_SO card = Results[j] as NonGod_Card_SO;
                    if (!card.name.Equals(SearchOptions[i]))
                        shouldBeRemoved = false;
                    
                    if (!card.type.ToString().Equals(SearchOptions[i]))
                        shouldBeRemoved = false;

                    if (!card.correspondingGod.ToString().Equals(SearchOptions[i]))
                        shouldBeRemoved = false;

                    if(shouldBeRemoved)
                        Results.RemoveAt(j);
                }
            }
        }
        return Results as List<Card_SO>;
    }
}
