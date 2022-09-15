using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_Behaviour : NonGod_Behaviour
{
    bool selectedToBuff = false;
    Buff_Card currentCard;

    public void Initialize(Buff_Card card)
    {
        strengh = card.baseStrengh;
        currentCard = card;
    }

    public int GetBuffedStat(int orStrengh)
    {
        selectedToBuff = true;
        if (currentCard.isMult)
            return orStrengh * strengh;
        else
            return orStrengh + strengh;
    }

    public override void OnAction()
    {
        //????????????
    }

    public override IEnumerator OnPlay(List<Enemy> enemies, List<NonGod_Behaviour> currLane, PlayerController player, God_Behaviour god)
    {
        foreach (NonGod_Behaviour card in currLane)
        {
            card.CanBeBuffedBy(this);
        }

        yield return new WaitUntil(() => { return selectedToBuff; });

        selectedToBuff = false;

        manager.FinishedPlay(this);
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

