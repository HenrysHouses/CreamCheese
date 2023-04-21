using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnemyAIEnums;

public class CleanseEnemyAction : MonsterAction
{

    public CleanseEnemyAction(int minimumStrength, int maximumStrength) : base(minimumStrength, maximumStrength)
    {
        ActionID = (int)EnemyIntent.CleanseEnemy;
        actionIcon = Resources.Load<Sprite>("EnemyData/Icons/Icon_Sword_IMG_v1");
        desc = "This enemy will remove all debuffs on all enemies";
        ActionIntentType = IntentType.Special;
    }

    public override void PerformAction(BoardStateController _board, int _strength, object _source)
    {

        Monster[] _enemies = _board.getLivingEnemies();

        if(_enemies.Length > 1)
        {
            
            for(int i = 0; i < _enemies.Length; i++)
            {

                _enemies[i].RemoveDebuffs();

            }

        }
        else
            _enemies[0].RemoveDebuffs();

        Monster _enemy = _source as Monster;
        if(_enemy)
        {
            _enemy.PlaySound(ActionSFX);
        }
    }
    
}