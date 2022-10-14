using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Intent
{
    public EnemyIntent GetCurrIntent()
    {
        EnemyIntent _i = (EnemyIntent)currIntent;
        return _i; 
    }
    int currIntent;
    List<Action> Actions;
    
    public void AddIntent(Action possibleAction)
    {
        if(possibleAction == null)
            return;

        // Add the intent to the List if its not inside it
        foreach (Action existingAction in Actions)
        {
            if (existingAction.ID == possibleAction.ID)
            {
                Debug.LogError("FSM ERROR: Trying to add a state that was already inside the list");
                return;
            }
        }

        Actions.Add(possibleAction);
    }

    public abstract void DecideIntent();

    public void Act()
    {
        Actions[currIntent].Execute();
    }
}

public enum EnemyIntent
{
    Attack,
    Defend,
    None,
}