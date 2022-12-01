/*
 * Written by:
 * Henrik
 * 
 */

public class WealthRune : rune
{
    public override void RuneEffect(TurnController controller)
    {
        if(hasTriggeredThisTurn)
            return;

        controller.Draw(RuneData.Strength);
        triggerOnceEachTurn();
    }

    public WealthRune(int str, RuneState state) : base(str, state)
    {
        string desc = "Draw one extra card each turn";

        this.RuneData = new RuneData(RuneType.FeWealth, desc, CombatState.DrawStep);
        this.RuneData.Strength = str;
        this.RuneData.State = state;
    }

    public WealthRune(int str) : base(str)
    {
        string desc = "Draw one extra card each turn";

        this.RuneData = new RuneData(RuneType.FeWealth, desc, CombatState.DrawStep);
        this.RuneData.Strength = str;
        this.RuneData.State = RuneState.Active;
    }
}