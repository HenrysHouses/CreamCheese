
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LokiMonster2 : IMonster
{
    IMonster toDefend;
    bool playerAttacked;

    public override void IsObjectiveTo(Attack_Behaviour attack_Behaviour)
    {
        attacker = attack_Behaviour;
        playerAttacked = true;
        //Debug.Log(this + " can be attacked by " + attack_Behaviour);
    }
    protected override bool UsesAbility(BoardState board)
    {
        if (getGod)
        {
            if (Random.Range(0, 100) < 30) //If God card is in play 33% chance to attack that instead of player
            {
                attackingPlayer = false;
            }
        }

        if (playerAttacked && GetMaxHealth() > GetHealth())
        {
            playerAttacked = false;
            toDefend = this;
            return true;
        }
        playerAttacked = false;

        List<IMonster> weakMonsters = new();
        foreach (IMonster a in board.enemies)
        {
            if(a != null)
            {
                if(a.attackingPlayer)
                {
                    if(Random.Range(0,100) < 25)
                    {
                        DealDamage(intentStrengh + 2);

                    }
                }
            }
        }
        if (weakMonsters.Count > 0 && UnityEngine.Random.Range(0, 2) == 1)
        {
            toDefend = weakMonsters[UnityEngine.Random.Range(0, weakMonsters.Count)];
            return true;
        }
        else
        {
            return false;
        }
    }
    protected override void AbilityDecided(BoardState board)
    {
        intentStrengh = 2;
        toDefend.Defend(intentStrengh);
    }

}
