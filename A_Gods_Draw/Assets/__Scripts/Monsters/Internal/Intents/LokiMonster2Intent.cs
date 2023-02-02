// Written by Javier

using System.Collections.Generic;
using UnityEngine;

public class LokiMonster2Intent : Intent
{
    List<Action> Actions = new List<Action>();

    public LokiMonster2Intent()
    {
        int scale = GameManager.timesDefeatedBoss;

        Actions.Add(new AttackGodAction(3 + scale, 5 + scale));
        Actions.Add(new AttackPlayerAction(3 + scale, 5 + scale));
        Actions.Add(new BuffAttackersAction(2 + scale, 2 + scale));
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
    }
    public override void LateDecideIntent(BoardStateController board)
    {
        if (actionSelected == null)
        {
            foreach (IMonster a in board.getLivingEnemies())
            {
                if (a == null)
                    continue;

                Intent _intent = a.GetIntent();
                if (_intent == null)
                    continue;

                if (_intent.GetID() != EnemyIntent.AttackPlayer || _intent.GetID() != EnemyIntent.AttackGod)
                    continue;

                if (UnityEngine.Random.Range(0, 4) < 3)
                {
                    actionSelected = GetAction<BuffAttackersAction>();
                }
                break;
            }
        }
        if (actionSelected == null)
        {
            actionSelected = GetAction<AttackPlayerAction>();
        }
        // Debug.Log( actionSelected);
        strengh = Random.Range(actionSelected.MinStrength, actionSelected.MaxStrength + 1);
    }
}