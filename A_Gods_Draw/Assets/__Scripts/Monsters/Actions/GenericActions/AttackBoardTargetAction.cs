using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using EnemyAIEnums;

public class AttackBoardTargetAction : MonsterAction
{

    public AttackBoardTargetAction(int minimumStrength, int maximumStrength) : base(minimumStrength, maximumStrength)
    {

        ActionID = (int)EnemyIntent.AttackPlayer;
        actionIcon = Resources.Load<Sprite>("EnemyData/Icons/Icon_AttackingChain_IMG_v1");
        desc = "Fenrir will attack the chains on the board";
        ActionIntentType = IntentType.Attack;

    }

    public override void SelectTargets(BoardStateController _board)
    {

        ResetTargets();

        List<BoardTarget> _targets = _board.ActiveExtraEnemyTargets;

        if(_targets.Count == 0)
        {

            Self.CancelIntent();
            return;

        }

        int _randomNum = Random.Range(0, _targets.Count);

        ITargets.Add(_targets[_randomNum]);
        _targets[_randomNum].Targeted(Self.gameObject);
        _targets.RemoveAt(_randomNum);

        _randomNum = Random.Range(0, _targets.Count);

        ITargets.Add(_targets[_randomNum]);
        _targets[_randomNum].Targeted(Self.gameObject);
        _targets.RemoveAt(_randomNum);

    }

    public override void PerformAction(BoardStateController _board, int _strength, object _source)
    {

        for(int i = 0; i < ITargets.Count; i++)
        {

            if(ITargets[i] == null)
                continue;
            
            ITargets[i].DealDamage(_strength);

        }

        ResetTargets();

        Monster _enemy = _source as Monster;
        if(_enemy)
        {
            _enemy.Animator.SetTrigger("Attack");
            _enemy.PlaySound(ActionSFX);
        }

    }

}
