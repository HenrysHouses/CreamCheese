using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LokiMonster2Intent : Intent
{
    List<Action> Actions = new List<Action>();

    public LokiMonster2Intent()
    {
        Actions.Add(new AttackGodAction(2, 4));
        Actions.Add(new AttackPlayerAction(2, 4));
        Actions.Add(new BuffAttackersAction(2, 2));
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

        actionSelected = GetAction<AttackPlayerAction>();
    }
    public override void LateDecideIntent(BoardStateController board)
    {
        if (!actionSelected)
        {
            foreach (IMonster a in board.Enemies)
            {
                if(a == null)
                    continue;

                Intent _intent = a.GetIntent();
                if(_intent == null)
                    continue;

                // Debug.Log(_intent);
                // Debug.Log(_intent.GetID());

                if (_intent.GetID() != EnemyIntent.AttackPlayer || _intent.GetID() != EnemyIntent.AttackGod)
                    continue;

                if (UnityEngine.Random.Range(0, 4) < 3)
                {
                    actionSelected = GetAction<BuffAttackersAction>();
                }
                break;
            }
        }
        // Debug.Log( actionSelected);
        strengh = Random.Range(actionSelected.Min(), actionSelected.Max() + 1);
    }
}
