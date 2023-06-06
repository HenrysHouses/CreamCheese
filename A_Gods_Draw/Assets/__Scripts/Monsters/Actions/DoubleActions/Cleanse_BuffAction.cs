using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnemyAIEnums;

public class Cleanse_BuffAction : MonsterAction
{

    public Cleanse_BuffAction(int minimumStrength, int maximumStrength) : base(minimumStrength, maximumStrength)
    {
        ActionID = (int)EnemyIntent.CleanseEnemy;
        actionIcon = Resources.Load<Sprite>("EnemyData/Icons/Icon_Cleanse_Buff_IMG_v1");
        desc = "This enemy will remove 1 debuff on an enemy and buff them for x";
        ActionIntentType = IntentType.Special;
    }

    public override void PerformAction(BoardStateController _board, int _strength, object _source)
    {

        if(MonsterTargets[0].TryGetComponent<DebuffBase>(out DebuffBase _debuff))
            _debuff.RemoveDebuff();

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
        
        MonsterTargets[0].TargetedByEnemy(Self, Color.yellow + (Color.red + Color.blue));
        Self.AddTargetEnemy(MonsterTargets[0]);

    }

}
