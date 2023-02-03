// Written by Javier Villegas
using System.Collections;
using UnityEngine;

public class AttackCardAction : CardAction
{

    //Onlane placed check for buff and buff cards when force of will placed

    public override IEnumerator OnAction(BoardStateController board, NonGod_Behaviour source)
    {
        isReady = false;

        yield return new WaitForSeconds(0.2f);
        foreach (IMonster target in source.stats.Targets)
        {
            if (target)
            {
                // Playing VFX for each action
                board.StartCoroutine(playTriggerVFX(source.gameObject, target));
                yield return new WaitUntil(() => !_VFX.isAnimating);
                target.DealDamage(source.stats.strength);
                yield return new WaitForSeconds(0.1f);
            }
        }

        source.stats.Targets.Clear();

        isReady = true;
    }

    public override void OnLanePlaced(BoardStateController _board, NonGod_Behaviour _source)
    {
        
        foreach(NonGod_Behaviour _card in _board.placedCards)
        {

            foreach(CardActionData _action in _card.stats.actionGroup.actionStats)
            {

                if(_action.actionEnum == CardActionEnum.BuffAll)
                {

                    currentCard.Buff(_card.stats.strength, false);

                }

            }

        }

    }

}
