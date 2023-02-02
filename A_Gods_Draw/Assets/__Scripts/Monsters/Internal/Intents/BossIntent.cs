// Written by Javier

using UnityEngine;

public class BossIntent : Intent
{
    AttackGodAction attackGod;
    AttackPlayerAction attackPlayer;
    public BossIntent()
    {
        int scale = GameManager.timesDefeatedBoss;

        // attackGod = new(6 + scale, 10 + scale * 2);
        // attackPlayer = new(6 + scale, 10 + scale * 2);
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
