using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack_Behaviour : NonGod_Behaviour
{
    Enemy a;
    int strengh;
    Attack_Card currentCard;

    public void Initialize(Attack_Card card)
    {
        strengh = card.baseStrengh;
        currentCard = card;
    }

    public new void OnAction()
    {
        //enemy.dealdamage
    }

    public new void OnPlay()
    {
        //Select enemy (enemy = manager.getclickedenemy())
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
