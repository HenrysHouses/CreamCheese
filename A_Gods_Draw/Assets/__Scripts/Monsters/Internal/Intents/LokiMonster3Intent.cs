using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LokiMonster3Intent : Intent
{
    AttackGodAction attackGod;
    AttackPlayerAction attackPlayer;
    public LokiMonster3Intent()
    {
        attackGod = new(5, 8);
        attackPlayer = new(5, 8);
    }

    public override void DecideIntent(BoardStateController board)
    {
        if (board.playedGodCard)
        {
            actionSelected = attackGod;
        }
        else
        {
            actionSelected = attackPlayer;
        }

        strengh = Random.Range(actionSelected.Min(), actionSelected.Max() + 1);
    }
}
