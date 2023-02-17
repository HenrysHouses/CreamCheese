// Written by Javier
/*
using System.Collections.Generic;
using UnityEngine;

public class LokiMonter1Intent : Intent
{
    AttackGodAction attackGod;
    AttackPlayerAction attackPlayer;
    DefendAction defend;
    public LokiMonter1Intent()
    {
        int scale = GameManager.timesDefeatedBoss;

        attackGod = new AttackGodAction(1 + scale, 3 + scale);
        attackPlayer = new AttackPlayerAction(3 + scale, 6 + scale * 2);
        defend = new DefendAction(2, 2 + scale);
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
            List<Monster> weakMonsters = new();
            foreach (Monster a in board.Enemies)
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
        strength = Random.Range(actionSelected.MinStrength, actionSelected.MaxStrength + 1);

        Just for safekeeping
        if (actionSelected == null)
        {
            foreach (Monster a in board.getLivingEnemies())
            {
                if (a == null)
                    continue;

                Intent _intent = a.GetIntent();
                if (_intent == null)
                    continue;

                if (_intent.GetID() != EnemyIntent.AttackPlayer || _intent.GetID() != EnemyIntent.AttackGod)
                    continue;

                if (UnityEngine.Random.Range(0, 4) < 3)
                {
                    actionSelected = GetAction<BuffAttackersAction>();
                }
                break;
            }
        }

    }
}
*/