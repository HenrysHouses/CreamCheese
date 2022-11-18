public class ChaosRune : rune
{
    public override void RuneEffect(TurnController controller)
    {
        // controller.Draw(RuneData.Strength);
    }

    public ChaosRune(int str, RuneState state) : base(str, state)
    {
        this.RuneData = new RuneData(RuneType.FeWealth, CombatState.DrawStep);
        this.RuneData.Strength = str;
        this.RuneData.State = state;
    }

    public ChaosRune(int str) : base(str)
    {
        this.RuneData = new RuneData(RuneType.FeWealth, CombatState.DrawStep);
        this.RuneData.Strength = str;
        this.RuneData.State = RuneState.Active;
    }
}
