using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnemyAIEnums;

public class AttackGod_DefendAction : MonsterAction
{
    public AttackGod_DefendAction(int minimumStrength, int maximumStrength) : base(minimumStrength, maximumStrength)
    {
        ActionID = (int)EnemyIntent.AttackGod;
        actionIcon = Resources.Load<Sprite>("EnemyData/Icons/Icon_Shield_AttackGod_IMG_v1");
        desc = "This enemy will attack the God card in play and defend itself";
        ActionIntentType = IntentType.Attack;
    }

    public override void PerformAction(BoardStateController _board, int _strength, object _source)
    {

        if (_board.playedGodCard != null)
        {

            _board.playedGodCard.DealDamage(_strength, _source as UnityEngine.Object);
            Self.Defend(_strength);

            ResetTargets();

            Monster _enemy = _source as Monster;
            if(_enemy)
            {
                _enemy.Animator.SetTrigger("Attack");
                _enemy.PlaySound(ActionSFX);
            }

        }        

    }

    public override void SelectTargets(BoardStateController _board)
    {

        ResetTargets();
        
        if(ITargets.Count > 0)
        {

            foreach (IMonsterTarget _target in ITargets)
            {

                _target.UnTargeted();
                
            }
            ITargets.Clear();

        }

        ITargets.Add(_board.playedGodCard);

        _board.playedGodCard.Targeted();
        Self.AddMonsterTarget(_board.playedGodCard);

    }

}
