using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnemyAIEnums;
using System.Linq;

public class MultiBuffEnemyAction : MonsterAction
{

    public MultiBuffEnemyAction(int minimumStrength, int maximumStrength) : base(minimumStrength, maximumStrength)
    {
        ActionID = (int)EnemyIntent.BuffAll;
        actionIcon = Resources.Load<Sprite>("EnemyData/Icons/Icon_Buff_IMG_v11");
        desc = "This enemy will buff all enemies";
        ActionIntentType = IntentType.Buff;
    }

    public override void PerformAction(BoardStateController _board, int _strength, object _source)
    {

        if(MonsterTargets.Count == 0)
            return;
        
        for(int i = 0; i < MonsterTargets.Count; i++)
        {

            MonsterTargets[i].Buff(_strength);
            if(ActionSettings.ActionVFX)
                GameObject.Instantiate(ActionSettings.ActionVFX, MonsterTargets[i].transform.position, Quaternion.identity);

        }

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
        
        MonsterTargets = _board.getLivingEnemies().ToList();
        MonsterTargets.Remove(Self);

        if(MonsterTargets.Count <= 0)
        {

            Self.CancelIntent();
            return;

        }

        for(int i = 0; i < MonsterTargets.Count; i++)
        {

            MonsterTargets[i].TargetedByEnemy(Self, Color.red + Color.blue);
            Self.AddTargetEnemy(MonsterTargets[i]);

        }

    }

}
