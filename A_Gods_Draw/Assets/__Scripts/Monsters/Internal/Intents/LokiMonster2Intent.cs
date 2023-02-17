// Written by Javier

using System.Collections.Generic;
using UnityEngine;

public class LokiMonster2Intent : Intent
{
    List<Action> Actions = new List<Action>();

    public LokiMonster2Intent()
    {
        int scale = GameManager.timesDefeatedBoss;

        Actions.Add(new AttackGodAction(1 + scale, 4 + scale));
        Actions.Add(new AttackPlayerAction(1 + scale, 4 + scale));
        Actions.Add(new BuffAttackersAction(2 + scale, 2 + scale));
        Actions.Add(new DefendAction(1, 4 + scale));
    }

    public T GetAction<T>() where T : Action
    {
        for (int i = 0; i < Actions.Count; i++)
        {
            if(Actions[i] is T)
                return Actions[i] as T;
        }
        return null;
    }

    public override void DecideIntent(BoardStateController board)
    {
        if (board.playedGodCard)
        {
            if (Random.Range(0, 100) < 33) //If God card is in play 33% chance to attack that instead of player
            {
                actionSelected = GetAction<AttackGodAction>();
                return;
            }
        }
        if (board.getLivingEnemies().Length == 1)
        {

            actionSelected = GetAction<AttackPlayerAction>();

        }
        else
        {

            if(Random.Range(0, 3) == 0 && PreviousAction != GetAction<DefendAction>())
            {

                GetAction<DefendAction>().toDefend = Self;
                actionSelected = GetAction<DefendAction>();

            }
            else
                actionSelected = GetAction<AttackPlayerAction>();

        }
    }
    public override void LateDecideIntent(BoardStateController _board)
    {

        if (actionSelected == null)
        {
            
            actionSelected = GetAction<AttackPlayerAction>();

        }
        
        strength = Random.Range(actionSelected.MinStrength, actionSelected.MaxStrength + 1);
        // Debug.Log("strength is: " + strength + " | Minimum: " + actionSelected.MinStrength + " | Maximum: " + actionSelected.MaxStrength + " | Selected Action: " + actionSelected.Explanation);
        PreviousAction = actionSelected;

    }
}