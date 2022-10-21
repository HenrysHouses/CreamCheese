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
                    // if(isGod)
                    // {
                    //     if(!Results[i].name.Equals(SearchOptions[j])) 
                    //         Results.RemoveAt(i);
                    // }
                    // else
                    // {
                    //     NonGod_Card card = Results[i] as NonGod_Card;
                    //     if(!card.name.Equals(SearchOptions[j])) 
                    //         Results.RemoveAt(i);

                    //     if(!card.cardType.ToString().Equals(SearchOptions[j])) 
                    //         Results.RemoveAt(i);
                    // }
                }
            }
        }
        return Results as List<Card_SO>;
    }
}
