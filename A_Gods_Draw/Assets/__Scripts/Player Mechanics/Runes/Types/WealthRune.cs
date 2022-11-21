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
        this.RuneData = new RuneData(RuneType.FeWealth, CombatState.DrawStep);
        this.RuneData.Strength = str;
        this.RuneData.State = state;
    }

    public WealthRune(int str) : base(str)
    {
        this.RuneData = new RuneData(RuneType.FeWealth, CombatState.DrawStep);
        this.RuneData.Strength = str;
        this.RuneData.State = RuneState.Active;
    }
}