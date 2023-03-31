using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FenrirDoubleAttackAction : MonsterAction
{

    public FenrirDoubleAttackAction(int minimumStrength, int maximumStrength) : base(minimumStrength, maximumStrength)
    {

        ActionID = (int)EnemyIntent.AttackPlayer;
        actionIcon = Resources.Load<Sprite>("EnemyData/Icons/Icon_Sword_IMG_v1");
        desc = "Fenrir will attack you or the chains at random twice, the more chains he breaks the more he will attack you";
        ActionIntentType = IntentType.Attack;

    }

    public override void SelectTargets(BoardStateController _board)
    {
        
        BoardTarget[] _targets = _board.ActiveExtraEnemyTargets.ToArray();

        int _chance = Random.Range(0, 4);

        if(_chance > _targets.Length - 1)
        {

            if(_board.isGodPlayed && Random.Range(0, 2) == 0)
                Targets.Add(_board.playedGodCard);
            else
                Targets.Add(_board.Player);

        }
        else
            Targets.Add(_targets[Random.Range(0, _targets.Length)]);
        
        _chance = Random.Range(0, 4);

        if(_chance > _targets.Length - 1)
        {

            if(_board.isGodPlayed && Random.Range(0, 2) == 0)
                Targets.Add(_board.playedGodCard);
            else
                Targets.Add(_board.Player);

        }
        else
            Targets.Add(_targets[Random.Range(0, _targets.Length)]);

        foreach (IMonsterTarget _target in Targets)
        {

            _target.Targeted();
            
        }

    }

    public override void Execute(BoardStateController _board, int _strength, Object _source = null)
    {

        for(int i = 0; i < Targets.Count; i++)
        {

            if(Targets[i] != null)
                Targets[i].DealDamage(_strength);

        }

        Targets.Clear();

        Monster _enemy = _source as Monster;
        if(_enemy)
            _enemy.animator.SetTrigger("Attack");

    }

}