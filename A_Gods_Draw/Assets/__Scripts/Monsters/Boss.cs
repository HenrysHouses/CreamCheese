// Written by Javier

public class Boss : Monster
{
    private void Start()
    {
        enemyIntent = new BossIntent();
    }

    // public override string setClassName()
    // {
    //     return typeof(Boss).Name;
    // }
}