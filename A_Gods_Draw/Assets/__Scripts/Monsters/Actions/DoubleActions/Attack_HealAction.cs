using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnemyAIEnums;

public class Attack_HealAction : MonsterAction
{

    public Attack_HealAction(int minimumStrength, int maximumStrength) : base(minimumStrength, maximumStrength)
    {
        ActionID = (int)EnemyIntent.Attack_Heal;
        actionIcon = Resources.Load<Sprite>("EnemyData/Icons/Icon_Heart_IMG_v1");
        desc = "This enemy will attack the player\nand heal the lowest health enemy";
        ActionIntentType = IntentType.Attack;
    }

    public override void PerformAction(BoardStateController _board, int _strength, object _source)
    {

        MonsterTargets[0].ReceiveHealth(_strength);
        if(ActionSettings.ActionVFX)
            GameObject.Instantiate(ActionSettings.ActionVFX, MonsterTargets[0].transform.position, Quaternion.identity);

        ResetTargets();

        _board.Player.DealDamage(_strength);

        Monster _enemy = _source as Monster;
        if(_enemy)
        {

            _enemy.Animator.SetTrigger("Attack");
            _enemy.PlaySound(ActionSFX);

        }

    }

    public override void SelectTargets(BoardStateController _board)
    {

        ResetTargets();

        Monster[] _enemies = _board.getLivingEnemies();

        if(_enemies.Length == 0)
            return;

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

        MonsterTargets.Add(_lowEnemy);

        _lowEnemy.TargetedByEnemy(Self, Color.green);
        Self.AddTargetEnemy(_lowEnemy);

    }

}
