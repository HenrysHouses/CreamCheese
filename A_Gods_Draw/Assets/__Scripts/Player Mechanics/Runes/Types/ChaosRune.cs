public class ChaosRune : rune
{
    public override void RuneEffect(TurnController controller)
    {
        if(hasTriggeredThisGame)
            return;

        IMonster[] currEnemies = controller.GetBoard().Enemies;

        foreach (var enemy in currEnemies)
        {
            enemy.DealDamage(this.RuneData.Strength);
        }

        triggerOnceEachGame();
    }

    public ChaosRune(int str, RuneState state) : base(str, state)
    {
        this.RuneData = new RuneData(RuneType.TursChaos, CombatState.DrawStep);
        this.RuneData.Strength = str;
        this.RuneData.State = state;
    }

    public ChaosRune(int str) : base(str)
    {
        this.RuneData = new RuneData(RuneType.TursChaos, CombatState.DrawStep);
        this.RuneData.Strength = str;
        this.RuneData.State = RuneState.Active;
    }
}
