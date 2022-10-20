using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LokiMonster3 : IMonster
{
    IMonster toDefend;
    bool playerAttacked;

    public override void IsObjectiveTo(Attack_Behaviour attack_Behaviour)
    {
        attacker = attack_Behaviour;
        playerAttacked = true;
        //Debug.Log(this + " can be attacked by " + attack_Behaviour);
    }
    protected override bool UsesAbility(BoardStateController board)
    {
        if(getGod)
        {
            // attackingPlayer = false;
        }
        
        
         playerAttacked = false;
          return false;
    } 
}
