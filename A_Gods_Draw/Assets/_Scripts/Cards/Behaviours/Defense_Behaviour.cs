using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defense_Behaviour : NonGod_Behaviour
{
    //Damageable a;
    int strengh;
    Defense_Card currentCard;

    public void Initialize(Defense_Card card)
    {
        strengh = card.baseStrengh;
        currentCard = card;
    }

    public new void OnAction()
    {
        //????????????
    }

    public new void OnPlay()
    {
        //Select player/god (Damageable = manager.selectwhotodefend())
        //a.defend
    }


    public new void GetBuff(bool isMultiplier, short amount)
    {
        if (isMultiplier)
        {
            strengh *= amount;
        }
        else
        {
            strengh += amount;
        }
    }

    public new void GetGodBuff(bool isMultiplier, short amount)
    {
        //if (manager.currentgod == currentcard.god)
        if (isMultiplier)
        {
            strengh *= amount;
        }
        else
        {
            strengh += amount;
        }
    }
}

