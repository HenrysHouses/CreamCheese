using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_Behaviour : NonGod_Behaviour
{
    NonGod_Behaviour a;
    short strengh;
    Buff_Card currentCard;

    public void Initialize(Buff_Card card)
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
        //Select card (a = manager.selectNonGod())
        a.GetBuff(currentCard.isMult, strengh);
    }


    public new void GetBuff(bool isMultiplier, short amount)
    {
        //if (isMultiplier)
        //{
        //    strengh *= amount;
        //}
        //else
        //{
        //    strengh += amount;
        //}
    }

    public new void GetGodBuff(bool isMultiplier, short amount)
    {
        ////if (manager.currentgod == currentcard.god)
        //if (isMultiplier)
        //{
        //    strengh *= amount;
        //}
        //else
        //{
        //    strengh += amount;
        //}
    }
}

