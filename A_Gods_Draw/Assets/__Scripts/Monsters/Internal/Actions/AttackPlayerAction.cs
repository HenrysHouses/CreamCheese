// Written by Javier

using UnityEngine;

public class AttackPlayerAction : Action
{
    public AttackPlayerAction()
    {
        ActionID = (int)EnemyIntent.AttackPlayer;
        actionIcon = Resources.Load<Sprite>("EnemyData/Icons/Icon_Sword_IMG_v1");
        desc = "This enemy will attack the player";
    }

    public override void Execute(BoardStateController BoardStateController, int strengh, UnityEngine.Object source)
    {
        BoardStateController.Player.DealDamage(strengh);
    }
}
