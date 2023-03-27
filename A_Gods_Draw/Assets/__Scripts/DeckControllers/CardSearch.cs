/* 
 * Written by 
 * Henrik
*/

using System.Collections.Generic;
using UnityEngine;

/// <summary>Class for searching for cards</summary>
public static class CardSearch
{
    /// <summary>Search for any cards created in Assets/Resources/Cards/, Filter by Card's ScriptableObject types</summary>
    /// <param name="SearchOptions">Possible Inputs: Card Name, Card Type, Corresponding God</param>
    /// <returns>A list of all cards matching the search</returns>
    public static List<Card_SO> Search<T>(string[] SearchOptions = null) where T : Card_SO
    {
        List<Card_SO> outputResults = new List<Card_SO>();

        Card_SO[] allResults = Resources.LoadAll<Card_SO>("Cards");
        if(SearchOptions != null)
        {
            for (int j = 0; j <= allResults.Length-1; j++)
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
                    
                    ActionCard_ScriptableObject card = allResults[j] as ActionCard_ScriptableObject;

                    if(card)
                    {
                        if (card.cardStats.correspondingGod.ToString() == SearchOptions[i])
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