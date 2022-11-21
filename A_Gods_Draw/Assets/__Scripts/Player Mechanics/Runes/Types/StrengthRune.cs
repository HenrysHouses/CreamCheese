public class StrengthRune : rune
{
    public override void RuneEffect(TurnController controller)
    {
    }

    public StrengthRune(int str, RuneState state) : base(str, state)
    {
        this.RuneData = new RuneData(RuneType.FeWealth, CombatState.DrawStep);
        this.RuneData.Strength = str;
        this.RuneData.State = state;
    }

    public StrengthRune(int str) : base(str)
    {
        this.RuneData = new RuneData(RuneType.FeWealth, CombatState.DrawStep);
        this.RuneData.Strength = str;
        this.RuneData.State = RuneState.Active;
    }
}