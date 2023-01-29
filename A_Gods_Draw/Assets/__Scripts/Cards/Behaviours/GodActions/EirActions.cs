using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EirActions : GodCardAction
{

    public override void Execute(BoardStateController board, int strengh, UnityEngine.Object source) { }

    public override void OnPlay(BoardStateController board, int strength)
    {

        foreach (IMonster _monster in board.getLivingEnemies())
        {
            _monster.ReceiveHealth(strength);

            PoisonDebuff _poison;
            if(_monster.gameObject.TryGetComponent<PoisonDebuff>(out _poison))
            {

                _poison.Stacks += strength;

            }
            else
            {

                _monster.gameObject.AddComponent<PoisonDebuff>().Stacks = strength;

            }

        }

        board.Player.Heal(strength);

    }

}
