using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using EnemyAIEnums;

public class FenrirDoubleAttackAction : MonsterAction
{

    public FenrirDoubleAttackAction(int minimumStrength, int maximumStrength) : base(minimumStrength, maximumStrength)
    {

        ActionID = (int)EnemyIntent.AttackPlayer;
        actionIcon = Resources.Load<Sprite>("EnemyData/Icons/Icon_AttackingChain_IMG_v2");
        desc = "Fenrir will attack you or the chains at random twice,\nthe more chains he breaks the more he will attack you";
        ActionIntentType = IntentType.Attack;

    }

    public override void SelectTargets(BoardStateController _board)
    {

        ResetTargets();

        BoardTarget[] _targets = _board.ActiveExtraEnemyTargets.ToArray();

        int _chance = Random.Range(0, 4);
        int _tempIndex = 0;

        if(_chance > _targets.Length - 1)
        {

            if(_board.isGodPlayed && Random.Range(0, 2) == 0)
                ITargets.Add(_board.playedGodCard);
            else
                ITargets.Add(_board.Player);

        }
        else
        {

            _tempIndex = Random.Range(0, _targets.Length);
            ITargets.Add(_targets[_tempIndex]);

        }
        
        _chance = Random.Range(0, 4);

        if(_chance > _targets.Length - 1)
        {

            if(_board.isGodPlayed && Random.Range(0, 2) == 0)
                ITargets.Add(_board.playedGodCard);
            else
                ITargets.Add(_board.Player);

        }
        else
        {

            int _tempIndex2 = 0;
            if(_targets.Length > 1)
                do
                {
                    _tempIndex2 = Random.Range(0, _targets.Length);
                } while (_tempIndex == _tempIndex2);
            
            ITargets.Add(_targets[_tempIndex2]);

        }

        foreach (IMonsterTarget _target in ITargets)
        {

            _target.Targeted(Self.gameObject);
            
        }

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