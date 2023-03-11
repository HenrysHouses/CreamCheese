using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EirActions : GodCardAction
{
    public EirActions()
    {
        SetActionVFX();
    }

    public override void Execute(BoardStateController board, int strengh, UnityEngine.Object source) { }

    public override void OnPlay(BoardStateController board, int strength)
    {
        Monster[] enemies = board.getLivingEnemies();
        EffectedTargetsForVFX = enemies;

        foreach (Monster _monster in enemies)
        {
            _monster.ApplyBarrier(strength);

        }

        board.Player.Heal(strength);

        
        base.OnPlay(board, strength);
    }

    public override void SetActionVFX()
    {
        EnterTheBattleFieldVFX = Resources.Load<GameObject>("Action VFX/FloralBarrier_Ground_VFX");
        EffectedTargetVFX = Resources.Load<GameObject>("Action VFX/FloralBarrier_Enemy_VFX");
        ETB_VFX_Lifetime = 3;
        Effected_VFX_Lifetime = 7;
    }
}
