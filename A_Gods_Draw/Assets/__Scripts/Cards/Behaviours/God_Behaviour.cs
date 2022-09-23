using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class God_Behaviour : Card_Behaviour
{
    public void SearchToBuff(List<NonGod_Behaviour> currentLane)
    {
        
    }
    public void OnRetire(List<TurnManager.LaneInfo> currentLane)
    {
        //foreach (NonGod_Behaviour card in currentLane)
        //{
        //    card.GetGodBuff(true, 0.5f);
        //}
    }

    public virtual void OnTurnStart() { }

}
