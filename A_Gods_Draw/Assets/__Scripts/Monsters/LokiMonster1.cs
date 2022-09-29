using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LokiMonster1 : IMonster
{
    IMonster toDefend;
    bool playerAttacked;

    public override void IsObjectiveTo(Attack_Behaviour attack_Behaviour)
    {
        attacker = attack_Behaviour;
        playerAttacked = true;
        //Debug.Log(this + " can be attacked by " + attack_Behaviour);
    }
    protected override bool UsesAbility(List<IMonster> enemies, List<NonGod_Behaviour> currentLane, PlayerController player, God_Behaviour currentGod) 
    {
        if (playerAttacked && GetMaxHealth() > GetHealth())
        {
            playerAttacked = false;
            toDefend = this;
            return true;
        }
        playerAttacked = false;

        List<IMonster> weakMonsters = new();
        foreach (IMonster a in enemies)
        {
            if (a.GetHealth() < a.GetMaxHealth() * 0.75f)
            {
                weakMonsters.Add(a);
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
    protected override void AbilityDecided(List<IMonster> enemies, List<NonGod_Behaviour> currentLane, PlayerController player, God_Behaviour currentGod)
    {
        intentStrengh = 2;
        toDefend.Defend(intentStrengh);
    }

}
