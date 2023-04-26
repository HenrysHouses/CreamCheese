// Written by Javier

using UnityEngine;
using FMODUnity;
using EnemyAIEnums;
using System.Collections;

public class AttackGodAction : MonsterAction
{
    public AttackGodAction(int minimumStrength, int maximumStrength) : base(minimumStrength, maximumStrength)
    {
        ActionID = (int)EnemyIntent.AttackGod;
        actionIcon = Resources.Load<Sprite>("EnemyData/Icons/Icon_Sword_IMG_v1");
        desc = "This enemy will attack the God card in play";
        ActionIntentType = IntentType.Attack;
    }

    public override IEnumerator PerformAction(BoardStateController _board, int _strength, object _source)
    {
        if (_board.playedGodCard != null)
        {
            _board.playedGodCard.DealDamage(_strength, _source as UnityEngine.Object);
            Monster _enemy = _source as Monster;
            if(_enemy)
            {
                _enemy.animator.SetTrigger("Attack");
                _enemy.PlaySound(ActionSFX);
            }

        }

        yield return null;
        
    }
}
