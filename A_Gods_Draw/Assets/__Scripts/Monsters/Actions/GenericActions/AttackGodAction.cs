// Written by Javier

using UnityEngine;
using FMODUnity;

public class AttackGodAction : MonsterAction
{
    public AttackGodAction(int minimumStrength, int maximumStrength, EventReference sfx) : base(minimumStrength, maximumStrength, sfx)
    {
        ActionID = (int)EnemyIntent.AttackGod;
        actionIcon = Resources.Load<Sprite>("EnemyData/Icons/Icon_Sword_IMG_v1");
        desc = "This enemy will attack the God card in play";
        ActionIntentType = IntentType.Attack;
    }

    public override void Execute(BoardStateController BoardStateController, int strengh, UnityEngine.Object source)
    {
        if (BoardStateController.playedGodCard != null)
        {
            BoardStateController.playedGodCard.DealDamage(strengh, source);
            Monster _enemy = source as Monster;
            if(_enemy)
            {
                _enemy.animator.SetTrigger("Attack");
                _enemy.PlaySound(ActionSFX);
            }

        }
        
    }
}
