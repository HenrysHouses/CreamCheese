/*
 * Written by:
 * Henrik
 * 
 */

using UnityEngine;
public class WealthRune : rune
{
    GameObject VFX;
    public override void RuneEffect(TurnController controller)
    {
        if(hasTriggeredThisTurn)
            return;

        controller.Draw(RuneData.Strength);
        triggerOnceEachTurn();
        Instantiate(VFX);
    }

    public WealthRune(int str, RuneState state) : base(str, state)
    {
        string desc = "Draw one extra card each turn";

        this.RuneData = new RuneData(RuneType.FeWealth, desc, CombatState.DrawStep);
        this.RuneData.Strength = str;
        this.RuneData.State = state;

        VFX = Resources.Load<GameObject>("Action VFX/WealthRune_VFX");
    }

    public WealthRune(int str) : base(str)
    {
        string desc = "Draw one extra card each turn";

        this.RuneData = new RuneData(RuneType.FeWealth, desc, CombatState.DrawStep);
        this.RuneData.Strength = str;
        this.RuneData.State = RuneState.Active;

        VFX = Resources.Load<GameObject>("Action VFX/WealthRune_VFX");
    }
}