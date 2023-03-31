// Written by Javier

using UnityEngine;
using FMODUnity;

public class DefendAction : MonsterAction
{
    public Monster toDefend;

    public DefendAction(int minimumStrength, int maximumStrength) : base(minimumStrength, maximumStrength)
    {
        ActionID = (int)EnemyIntent.Defend;
        actionIcon = Resources.Load<Sprite>("EnemyData/Icons/Icon_Shield_IMG_v1");
        desc = "This enemy will defend an enemy for the next turn onward";
        ActionIntentType = IntentType.Defend;
    }

    public override void Execute(BoardStateController BoardStateController, int _strength, UnityEngine.Object source)
    {
        Self.Defend(_strength);
    }
}
