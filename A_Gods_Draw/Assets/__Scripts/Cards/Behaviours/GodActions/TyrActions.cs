// Written by Javier Villegas
using UnityEngine;

[System.Serializable]
public class TyrActions : GodCardAction
{
    public TyrActions()
    {
        SetActionVFX();
    }

    public override void Execute(BoardStateController board, int strength, UnityEngine.Object source) { }

    public override void OnPlay(BoardStateController board, int strength)
    {

        if(board.ActiveBattleFieldType == BattlefieldID.Fenrir)
        {

            BoardTarget[] _boardTargets = board.AllExtraEnemyTargets.ToArray();


            EffectedTargetsForVFX = _boardTargets;

            int _reActivations = 0;
            for(int i = 0; i < _boardTargets.Length; i++)
            {

                if(!_boardTargets[i].IsActive)
                {

                    _reActivations++;
                    _boardTargets[i].ReActivate();
                
                }

                if(_reActivations == 2)
                    break;

            }

        }
        else
        {
            Monster[] enemies = board.getLivingEnemies();
            EffectedTargetsForVFX = enemies;

            foreach (Monster monster in enemies)
            {
                DebuffBase _debuff = monster.gameObject.AddComponent<ChainedDebuff>();
                _debuff.Stacks = 1;
                _debuff.thisMonster = monster;
            }
        }
        base.OnPlay(board, strength);
    }

    public override void SetActionVFX()
    {
        EffectedTargetVFX = Resources.Load<GameObject>("Action VFX/Chained_VFX");
        Effected_VFX_Lifetime = 3;

        // EnterTheBattleFieldVFX = 
        // ETB_VFX_Lifetime =
    }
}
