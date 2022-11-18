public class WealthRune : rune
{
    protected override void RuneEffect(TurnController controller)
    {
        controller.Draw(RuneID.Strength);
    }

    public WealthRune(int str, RuneState state) : base(str, state)
    {
        this.RuneID = new RuneData(RuneType.FeWealth, CombatState.DrawStep);
        this.RuneID.Strength = str;
        this.RuneID.State = state;
    }

    public WealthRune(int str) : base(str)
    {
        this.RuneID = new RuneData(RuneType.FeWealth, CombatState.DrawStep);
        this.RuneID.Strength = str;
        this.RuneID.State = RuneState.Active;
    }
}