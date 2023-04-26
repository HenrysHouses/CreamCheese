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
        actionIcon = Resources.Load<Sprite>("EnemyData/Icons/Icon_Buff_IMG_v7");
        desc = "This enemy will buff a random enemy";
        ActionIntentType = IntentType.Buff;
    }

    public override IEnumerator PerformAction(BoardStateController _board, int _strength, object _source)
    {
        List<Monster> _monsters = _board.getLivingEnemies().ToList();
        _monsters.Remove(Self);

        if(_monsters.Count != 0)
        {
            int _targetIndex = Random.Range(0, _monsters.Count);
            _monsters[_targetIndex].Strengthen(_strength);
            if(ActionSettings.ActionVFX)
                GameObject.Instantiate(ActionSettings.ActionVFX, _monsters[_targetIndex].transform.position, Quaternion.identity);
        }

        yield return null;

    }
}
