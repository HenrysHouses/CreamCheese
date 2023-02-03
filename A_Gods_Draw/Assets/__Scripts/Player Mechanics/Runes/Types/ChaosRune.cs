/*
 * Written by:
 * Henrik
 * 
 */

public class ChaosRune : rune
{
    public override void RuneEffect(TurnController controller)
    {
        if(hasTriggeredThisGame)
            return;

        Monster[] currEnemies = controller.GetBoard().Enemies;

        foreach (var enemy in currEnemies)
        {
            enemy.DealDamage(this.RuneData.Strength);
        }

        triggerOnceEachGame();
    }

    public ChaosRune(int str, RuneState state) : base(str, state)
    {
        string desc = "Deal 1 damage to each enemy at start of combat";
        
        this.RuneData = new RuneData(RuneType.TursChaos, desc, CombatState.DrawStep);
        this.RuneData.Strength = str;
        this.RuneData.State = state;
    }

    public ChaosRune(int str) : base(str)
    {
        string desc = "Deal 1 damage to each enemy at start of combat";

        this.RuneData = new RuneData(RuneType.TursChaos, desc, CombatState.DrawStep);
        this.RuneData.Strength = str;
        this.RuneData.State = RuneState.Active;
    }
}
