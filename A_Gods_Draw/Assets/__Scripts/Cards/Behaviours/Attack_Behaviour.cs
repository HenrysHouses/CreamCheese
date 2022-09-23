using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack_Behaviour : NonGod_Behaviour
{
    List<IMonster> targets = new List<IMonster>();
    Attack_Card currentCard;

    public override void Initialize(Card_SO card)
    {
        current = (card as NonGod_Card);
        currentCard = card as Attack_Card;
        strengh = currentCard.baseStrengh;
        this.card = card;
    }

    public override void OnAction()
    {
        foreach (IMonster target in targets)
        {
            target.DealDamage(strengh);
            Debug.Log("Dealt " + strengh + " damage to " + target);
        }
    }

    public override IEnumerator OnPlay(List<IMonster> enemies, List<NonGod_Behaviour> currLane, PlayerController player, God_Behaviour god)
    {
        foreach (IMonster enemy in enemies)
        {
            enemy.IsObjectiveTo(this);
        }

        //Debug.Log("SelectEnemies");

        yield return new WaitUntil(() => { return targets.Count == 1; });

        manager.FinishedPlay(this);

        //Debug.Log("readyto act");
    }

    public void AddTarget(IMonster enemy)
    {
        targets.Add(enemy);
    }
}
