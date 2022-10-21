using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LokiMonster2Intent : Intent
{
    AttackGodAction attackGod;
    AttackPlayerAction attackPlayer;
    BuffAttackersAction buffA;
    public LokiMonster2Intent()
    {
        attackGod = new(2, 4);
        attackPlayer = new(2, 4);
        buffA = new(2, 2);
    }

    public override void DecideIntent(BoardStateController board)
    {
        if (board.playedGodCard)
        {
            if (Random.Range(0, 100) < 33) //If God card is in play 33% chance to attack that instead of player
            {
                actionSelected = attackGod;
            }
        }
    }
    public override void LateDecideIntent(BoardStateController board)
    {
        if (!actionSelected)
        {
            foreach (IMonster a in board.Enemies)
            {
                if (a != null)
                {
                    if (a.GetIntent().GetID() == EnemyIntent.AttackPlayer || a.GetIntent().GetID() == EnemyIntent.AttackGod)
                    {
                        if (UnityEngine.Random.Range(0, 4) < 3)
                        {
                            actionSelected = buffA;
                        }
                        else
                        {
                            actionSelected = attackPlayer;
                        }
                        break;
                    }
                }
            }
        }

        strengh = Random.Range(actionSelected.Min(), actionSelected.Max() + 1);
    }
}
