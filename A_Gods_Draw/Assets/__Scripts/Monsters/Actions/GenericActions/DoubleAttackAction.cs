using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class DoubleAttackAction : MonsterAction
{

    public DoubleAttackAction(int minimumStrength, int maximumStrength) : base(minimumStrength, maximumStrength)
    {

        ActionID = (int)EnemyIntent.AttackPlayer;
        actionIcon = Resources.Load<Sprite>("EnemyData/Icons/Icon_Sword_IMG_v1");
        desc = "This enemy will attack 2 random targets";
        ActionIntentType = IntentType.Attack;

    }

    public override void SelectTargets(BoardStateController _board)
    {
        
        Targets.Clear();
        if(_board.isGodPlayed)
        {

            for(int i = 0; i < 2; i++)
            {

                if(Random.Range(0, 2) == 0)
                    Targets.Add(_board.playedGodCard.GetComponent<IMonsterTarget>());
                else
                    Targets.Add(_board.Player.GetComponent<IMonsterTarget>());
                
            }

        }
        else
        {

            Targets.Add(_board.Player.GetComponent<IMonsterTarget>());
            Targets.Add(_board.Player.GetComponent<IMonsterTarget>());

        }

        foreach (IMonsterTarget _target in Targets)
        {

            _target.Targeted();
            
        }

    }

    public override void PerformAction(BoardStateController _board, int _strength, object _source)
    {

        for(int i = 0; i < Targets.Count; i++)
        {

            if(Targets[i] != null)
                Targets[i].DealDamage(_strength);

        }

        Targets.Clear();

        Monster _enemy = _source as Monster;
        if(_enemy)
        {
            _enemy.animator.SetTrigger("Attack");
            _enemy.PlaySound(ActionSFX);
        }

    }

}
