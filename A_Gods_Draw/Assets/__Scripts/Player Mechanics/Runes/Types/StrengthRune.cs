public class StrengthRune : rune
{
    public override void RuneEffect(TurnController controller)
    {
    }

    public StrengthRune(int str, RuneState state) : base(str, state)
    {
        string desc = "First card you play each combat is buffed by X";

        this.RuneData = new RuneData(RuneType.UrrStrength, desc, CombatState.DrawStep);
        this.RuneData.Strength = str;
        this.RuneData.State = state;
    }

    public StrengthRune(int str) : base(str)
    {
        string desc = "First card you play each combat is buffed by X";

        this.RuneData = new RuneData(RuneType.UrrStrength, desc, CombatState.DrawStep);
        this.RuneData.Strength = str;
        this.RuneData.State = RuneState.Active;
    }
}