using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using EnemyAIEnums;

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
        
        ITargets.Clear();
        if(_board.isGodPlayed)
        {

            for(int i = 0; i < 2; i++)
            {

                if(Random.Range(0, 2) == 0)
                    ITargets.Add(_board.playedGodCard.GetComponent<IMonsterTarget>());
                else
                    ITargets.Add(_board.Player.GetComponent<IMonsterTarget>());
                
            }

        }
        else
        {

            ITargets.Add(_board.Player.GetComponent<IMonsterTarget>());
            ITargets.Add(_board.Player.GetComponent<IMonsterTarget>());

        }

        foreach (IMonsterTarget _target in ITargets)
        {

            _target.Targeted();
            
        }

    }

    public override void PerformAction(BoardStateController _board, int _strength, object _source)
    {

        for(int i = 0; i < ITargets.Count; i++)
        {

            if(ITargets[i] != null)
                ITargets[i].DealDamage(_strength);

        }

        ITargets.Clear();

        Monster _enemy = _source as Monster;
        if(_enemy)
        {
            _enemy.animator.SetTrigger("Attack");
            _enemy.PlaySound(ActionSFX);
        }

    }

}
