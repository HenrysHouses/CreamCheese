using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnemyAIEnums;

public class HealEnemyAction : MonsterAction
{

    public HealEnemyAction(int minimumStrength, int maximumStrength) : base(minimumStrength, maximumStrength)
    {
        ActionID = (int)EnemyIntent.HealEnemy;
        actionIcon = Resources.Load<Sprite>("EnemyData/Icons/Icon_Heart_IMG_v1");
        desc = "This enemy will heal the enemy with the least health";
        ActionIntentType = IntentType.Special;
    }

    public override void PerformAction(BoardStateController _board, int _strength, object _source)
    {

        Monster[] _enemies = _board.getLivingEnemies();
        Monster _lowEnemy = _enemies[0];

        if(_enemies.Length > 1)
        {

            float _lowestHealth = (float)_enemies[0].GetHealth() / (float)_enemies[0].GetMaxHealth();
            
            for(int i = 0; i < _enemies.Length; i++)
            {

                float _tempHealth = (float)_enemies[i].GetHealth() / (float)_enemies[i].GetMaxHealth();

                if(_tempHealth >= _lowestHealth)
                    continue;

                _lowestHealth = _tempHealth;
                _lowEnemy = _enemies[i];

            }

        }

        _lowEnemy.ReceiveHealth(_strength);
        if(ActionSettings.ActionVFX)
            GameObject.Instantiate(ActionSettings.ActionVFX, _lowEnemy.transform.position, Quaternion.identity);

        Monster _enemy = _source as Monster;
        if(_enemy)
        {
            _enemy.PlaySound(ActionSFX);
        }
    }
    
}