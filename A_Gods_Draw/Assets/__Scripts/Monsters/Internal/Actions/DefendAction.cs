// Written by Javier

using UnityEngine;

public class DefendAction : MonsterAction
{
    public IMonster toDefend;

    public DefendAction(int minimumStrength, int maximumStrength) : base(minimumStrength, maximumStrength)
    {
        ActionID = (int)EnemyIntent.Defend;
        actionIcon = Resources.Load<Sprite>("EnemyData/Icons/Icon_Shield_IMG_v1");
        desc = "This enemy will defend an enemy for the next turn onward";
    }

    public override void Execute(BoardStateController BoardStateController, int strengh, UnityEngine.Object source)
    {
        if (toDefend)
            toDefend.Defend(strengh);
    }
}
