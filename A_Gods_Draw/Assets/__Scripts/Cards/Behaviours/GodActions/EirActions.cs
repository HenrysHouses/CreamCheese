using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EirActions : GodCardAction
{

    public override void Execute(BoardStateController board, int strengh, UnityEngine.Object source) { }

    public override void OnPlay(BoardStateController board, int strength)
    {

        foreach (IMonster monster in board.getLivingEnemies())
        {
            monster.ReceiveHealth(strength);
            monster.gameObject.AddComponent<PoisonDebuff>().stacks = strength;
        }

        board.Player.Heal(strength);

    }

}
