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

        for(int i = 0; i < Self.GetIntent().GetStrength; i++)
            MonsterTargets[0].GetComponent<DebuffBase>().RemoveDebuff();

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

        Monster[] _enemies = _board.getLivingEnemies();
        List<Monster> _debuffedEnemies = new List<Monster>();

        foreach(Monster _enemyToCheck in _enemies)
            if(_enemyToCheck.HasDebuffNextRound())
                _debuffedEnemies.Add(_enemyToCheck);

        if(_debuffedEnemies.Count == 0)
        {

            Self.CancelIntent();
            return;

        }

        MonsterTargets.Add(_debuffedEnemies[Random.Range(0, _debuffedEnemies.Count)]);
        
        MonsterTargets[0].TargetedByEnemy(Self, Color.yellow);
        Self.AddTargetEnemy(MonsterTargets[0]);

    }
    
}