using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack_Behaviour : NonGod_Behaviour
{
    List<Enemy> targets = new List<Enemy>();

    public void Initialize(Attack_Card card)
    {
        strengh = card.baseStrengh;
    }

    public override void OnAction()
    {
        foreach (Enemy target in targets)
        {
            target.DealDamage(strengh);
            Debug.Log("Dealt damage to " + target);
        }
    }

    public override IEnumerator OnPlay(List<Enemy> enemies, List<NonGod_Behaviour> currLane, PlayerController player, God_Behaviour god)
    {
        foreach (Enemy enemy in enemies)
        {
            enemy.IsObjectiveTo(this);
        }

        Debug.Log("SelectEnemies");

        yield return new WaitUntil(() => { return targets.Count == 1; });

        manager.FinishedPlay(this);

        Debug.Log("readyto act");
    }


    public new void GetBuff(bool isMultiplier, short amount)
    {
        if (isMultiplier)
        {
            strengh *= amount;
        }
        else
        {
            strengh += amount;
        }
    }

    public void AddTarget(Enemy enemy)
    {
        targets.Add(enemy);
    }

    public void GetGodBuff(bool isMultiplier, short amount)
    {
        //if (manager.currentgod == currentcard.god)
            if (isMultiplier)
            {
                strengh *= amount;
            }
            else
            {
                strengh += amount;
            }
    }
}
