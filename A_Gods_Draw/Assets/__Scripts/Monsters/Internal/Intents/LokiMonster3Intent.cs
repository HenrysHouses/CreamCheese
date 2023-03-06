// Written by Javier
/*
using UnityEngine;

public class LokiMonster3Intent : Intent
{
    AttackGodAction attackGod;
    AttackPlayerAction attackPlayer;
    public LokiMonster3Intent()
    {
        int scale = GameManager.timesDefeatedBoss;

        attackGod = new(5 + scale, 8 + scale);
        attackPlayer = new(5 + scale, 8 + scale * 2);
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

        strength = Random.Range(actionSelected.MinStrength, actionSelected.MaxStrength + 1);
    }
}
*/