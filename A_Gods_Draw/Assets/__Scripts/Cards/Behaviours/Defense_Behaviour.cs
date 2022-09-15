using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defense_Behaviour : NonGod_Behaviour
{
    //Damageable a;
    Defense_Card currentCard;

    public void Initialize(Defense_Card card)
    {
        strengh = card.baseStrengh;
        currentCard = card;
    }

    public override void OnAction()
    {
        //????????????
    }

    public override IEnumerator OnPlay(List<Enemy> enemies, List<NonGod_Behaviour> currLane, PlayerController player, God_Behaviour god)
    {
        //Select player/god (Damageable = manager.selectwhotodefend())
        //a.defend
        yield return new WaitUntil(() => { return true; });
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

