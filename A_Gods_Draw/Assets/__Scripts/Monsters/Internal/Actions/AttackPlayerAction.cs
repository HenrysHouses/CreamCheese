// Written by Javier

using UnityEngine;

[System.Serializable]
public class AttackPlayerAction : MonsterAction
{
    public AttackPlayerAction(int minimumStrength, int maximumStrength) : base(minimumStrength, maximumStrength)
    {
        ActionID = (int)EnemyIntent.AttackPlayer;
        actionIcon = Resources.Load<Sprite>("EnemyData/Icons/Icon_Sword_IMG_v1");
        desc = "This enemy will attack the player";
    }

    public override void Execute(BoardStateController BoardStateController, int strengh, UnityEngine.Object source)
    {
        BoardStateController.Player.DealDamage(strengh);

        Monster enemy = source as Monster;
        if(enemy)
            enemy.animator.SetTrigger("Attack");
    }
}
