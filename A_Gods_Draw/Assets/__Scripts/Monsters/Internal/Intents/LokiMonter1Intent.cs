using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LokiMonter1Intent : Intent
{
    AttackGodAction attackGod;
    AttackPlayerAction attackPlayer;
    DefendAction defend;
    public LokiMonter1Intent()
    {
        attackGod = new(1, 3);
        attackPlayer = new(1, 3);
        defend = new(2, 2);
    }

    public override void DecideIntent(BoardStateController board)
    {
        if (board.playedGodCard)
        {
            if (Random.Range(0, 100) < 50) //If God card is in play 50% chance to attack that instead of player
            {
                actionSelected = attackGod;
            }
        }

        if (actionSelected == null)
        {
            List<IMonster> weakMonsters = new();
            foreach (IMonster a in board.Enemies)
            {
                if (a.GetHealth() < a.GetMaxHealth() * 0.75f)
                {
                    weakMonsters.Add(a);
                }
            }
            if (weakMonsters.Count > 0 && UnityEngine.Random.Range(0, 2) == 1)
            {
                defend.toDefend = weakMonsters[UnityEngine.Random.Range(0, weakMonsters.Count)];
                actionSelected = defend;
            }
            else
            {
                actionSelected = attackPlayer;
            }
        }
        strengh = Random.Range(actionSelected.Min(), actionSelected.Max() + 1);
    }
}
