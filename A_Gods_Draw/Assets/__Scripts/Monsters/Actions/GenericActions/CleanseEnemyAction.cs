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
        desc = "This enemy will remove all debuffs on x enemies";
        ActionIntentType = IntentType.Special;
    }

    public override void PerformAction(BoardStateController _board, int _strength, object _source)
    {

        Monster[] _enemies = _board.getLivingEnemies();

        int _cleansed = 0;
        foreach(Monster _enemyToCheck in _enemies)
        {

            if(_cleansed == _strength)
                break;

            if(_enemyToCheck.HasDebuff())
            {

                _cleansed++;
                _enemyToCheck.RemoveDebuffs();
                if(ActionSettings.ActionVFX)
                    GameObject.Instantiate(ActionSettings.ActionVFX, _enemyToCheck.transform.position, Quaternion.identity);

            }

        }

        Monster _enemy = _source as Monster;
        if(_enemy)
        {
            _enemy.PlaySound(ActionSFX);
        }
    }
    
}