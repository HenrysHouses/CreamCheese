using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullCardList : MonoBehaviour
{

    void Start()
    {
        

        CardSearch(new God_Card(),  new string[] {"Tyr"});
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Card_SO CardSearch(Card_SO type, string[] SearchOptions = null)
    {
        List<Card_SO> Results = new List<Card_SO>();

        God_Card god = type as God_Card;
        NonGod_Card ActionCard = type as NonGod_Card;
        bool godSearch = false;


        if(god)
        {
            Card_SO[] currResults = Resources.LoadAll<God_Card>("Cards");

            for (int i = 0; i < currResults.Length; i++)
            {
                Results.Add(currResults[i]);
            }
            godSearch = true;

        } else if(ActionCard)
        {
            Card_SO[] currResults = Resources.LoadAll<Card_SO>("Cards");

            for (int i = 0; i <currResults.Length ; i++)
            {
                Results.Add(currResults[i]);
                
            }

        }

        if(SearchOptions != null)
        {
            for (int i = 0; i < SearchOptions.Length; i++)
            {
                for (int j = 0; j < Results.Count; j++)
                {
                    God_Card C = null; 
                    NonGod_Card N = null;
                    if(godSearch)
                    {
                        C = Results[i] as God_Card;

                        if(!C.name.Equals(SearchOptions[j])) 
                            Results.RemoveAt(i);
                    }
                    else
                    {
                        N = Results[i] as NonGod_Card;

                        if(!N.name.Equals(SearchOptions[j])) 
                            Results.RemoveAt(i);

                        if(!N.name.Equals(SearchOptions[j])) 
                            Results.RemoveAt(i);
                    }


                    
                }
            }
        }


        return null;
    }






}
