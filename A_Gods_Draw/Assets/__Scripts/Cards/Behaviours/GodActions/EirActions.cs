using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EirActions : GodCardAction
{

    public override void Execute(BoardStateController board, int strengh, UnityEngine.Object source) { }

    public override void OnPlay(BoardStateController board, int strength)
    {

        foreach (IMonster _monster in board.getLivingEnemies())
        {
            _monster.ApplyBarrier(strength);

        }

        board.Player.Heal(strength);

    }

}
