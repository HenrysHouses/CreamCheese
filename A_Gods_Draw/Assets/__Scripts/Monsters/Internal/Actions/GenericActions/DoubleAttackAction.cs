using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleAttackAction : MonsterAction
{

    public DoubleAttackAction(int minimumStrength, int maximumStrength) : base(minimumStrength, maximumStrength)
    {

        ActionID = (int)EnemyIntent.AttackPlayer;
        actionIcon = Resources.Load<Sprite>("EnemyData/Icons/Icon_Sword_IMG_v1");
        desc = "This enemy will attack the player";

    }

    public override void Execute(BoardStateController _board, int _strength, Object _source = null)
    {

        int _attacks = 0;
        BoardTarget[] _targets = _board.ActiveExtraEnemyTargets.ToArray();

        if(_board.ActiveBattleFieldType == BattlefieldID.Fenrir)
        {

            int _chance = Random.Range(0, 4);

            if(_chance > (_targets.Length-1))
            {

                _board.Player.DealDamage(_strength);
                
                if(_board.isGodPlayed)
                    _board.playedGodCard.DealDamage(_strength, _source);
                else
                    _board.Player.DealDamage(_strength);

            }
            else
            {

                _board.ActiveExtraEnemyTargets[Random.Range(0, _board.ActiveExtraEnemyTargets.Count)].TakeDamage(_strength);
                if(_board.ActiveExtraEnemyTargets.Count > 0)
                    _board.ActiveExtraEnemyTargets[Random.Range(0, _board.ActiveExtraEnemyTargets.Count)].TakeDamage(_strength);
                else if(Random.Range(0, 2) == 0)
                    _board.Player.DealDamage(_strength);
                else
                    _board.playedGodCard.DealDamage(_strength, _source);

            }

        }
        else
            while(_attacks < 2)
            {

                switch (Random.Range(0, 3))
                {
                    
                    case 0:
                    _board.Player.DealDamage(_strength);
                    _attacks += 1;
                    break;

                    case 1:
                    if(_board.isGodPlayed)
                    {
                        _board.playedGodCard.DealDamage(_strength, _source);
                        _attacks += 1;
                    }
                    break;

                    case 2:
                    if(_board.ActiveExtraEnemyTargets != null)
                    {
                        _board.ActiveExtraEnemyTargets[Random.Range(0, _board.ActiveExtraEnemyTargets.Count)].TakeDamage(_strength);
                        _attacks += 1;
                    }
                    break;
                    
                }

            }

        Monster _enemy = _source as Monster;
        if(_enemy)
            _enemy.animator.SetTrigger("Attack");

    }

}
