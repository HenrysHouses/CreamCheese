using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnemyAIEnums;

public class CleanseEnemyAction : MonsterAction
{

    public CleanseEnemyAction(int minimumStrength, int maximumStrength) : base(minimumStrength, maximumStrength)
    {
        ActionID = (int)EnemyIntent.CleanseEnemy;
        actionIcon = Resources.Load<Sprite>("EnemyData/Icons/CleanseIcon");
        desc = "This enemy will remove all debuffs on x enemies";
        ActionIntentType = IntentType.Special;
    }

    public override void PerformAction(BoardStateController _board, int _strength, object _source)
    {

        for(int i = 0; i < MonsterTargets.Count; i++)
        {

            MonsterTargets[i].RemoveDebuffs();
            if(ActionSettings.ActionVFX)
                GameObject.Instantiate(ActionSettings.ActionVFX, MonsterTargets[i].transform.position, Quaternion.identity);

        }

        Monster _enemy = _source as Monster;
        if(_enemy)
        {
            _enemy.PlaySound(ActionSFX);
        }

    }

    public override void SelectTargets(BoardStateController _board)
    {

        int _strength = Self.GetIntent().GetStrength;
        Monster[] _enemies = _board.getLivingEnemies();

        int _cleansed = 0;
        foreach(Monster _enemyToCheck in _enemies)
        {

            if(_cleansed == _strength)
                break;

            if(_enemyToCheck.HasDebuffNextRound())
            {

                _cleansed++;
                MonsterTargets.Add(_enemyToCheck);

                _enemyToCheck.TargetedByEnemy(Self, Color.yellow);

            }

        }

    }
    
}