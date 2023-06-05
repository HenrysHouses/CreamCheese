using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using EnemyAIEnums;

public class BuffEnemyAction : MonsterAction
{
    public BuffEnemyAction(int minimumStrength, int maximumStrength) : base(minimumStrength, maximumStrength)
    {
        ActionID = (int)EnemyIntent.BuffEnemy;
        actionIcon = Resources.Load<Sprite>("EnemyData/Icons/Icon_Buff_IMG_v11");
        desc = "This enemy will buff a random enemy";
        ActionIntentType = IntentType.Buff;
    }

    public override void PerformAction(BoardStateController _board, int _strength, object _source)
    {

        if(MonsterTargets.Count == 0)
            return;
        
        MonsterTargets[0].Buff(_strength);
        if(ActionSettings.ActionVFX)
            GameObject.Instantiate(ActionSettings.ActionVFX, MonsterTargets[0].transform.position, Quaternion.identity);

        ResetTargets();

        Monster _enemy = _source as Monster;
        if(_enemy)
        {
            _enemy.Animator.SetTrigger("Buffing");
            _enemy.PlaySound(ActionSFX);
        }

    }

    public override void SelectTargets(BoardStateController _board)
    {

        ResetTargets();
        
        List<Monster> _targets = _board.getLivingEnemies().ToList();
        _targets.Remove(Self);

        if(_targets.Count <= 0)
        {

            Self.CancelIntent();
            return;

        }

        MonsterTargets.Add(_targets[Random.Range(0, _targets.Count)]);

        MonsterTargets[0].TargetedByEnemy(Self, Color.red + Color.blue);
        Self.AddTargetEnemy(MonsterTargets[0]);

    }

}
