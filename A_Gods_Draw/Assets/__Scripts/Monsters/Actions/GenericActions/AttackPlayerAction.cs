// Written by Javier

using UnityEngine;
using FMODUnity;

[System.Serializable]
public class AttackPlayerAction : MonsterAction
{
    public AttackPlayerAction(int minimumStrength, int maximumStrength) : base(minimumStrength, maximumStrength)
    {
        ActionID = (int)EnemyIntent.AttackPlayer;
        actionIcon = Resources.Load<Sprite>("EnemyData/Icons/Icon_Sword_IMG_v1");
        desc = "This enemy will attack the player";
        ActionIntentType = IntentType.Attack;
    }

    public override void Execute(BoardStateController BoardStateController, int strengh, UnityEngine.Object source)
    {
        BoardStateController.Player.DealDamage(strengh);

        Monster _enemy = source as Monster;
        if(_enemy)
        {
            _enemy.animator.SetTrigger("Attack");
            _enemy.PlaySound(ActionSFX);
        }
    }
}
