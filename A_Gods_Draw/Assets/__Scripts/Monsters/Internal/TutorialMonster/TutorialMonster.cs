using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialMonster : Monster
{

    protected override void Start()
    {
        
        enemyIntent = new TutorialMonsterIntent(ref EnemyActions);
        enemyIntent.Self = this;
        healthBarColor = healthBarFill.color;
        barrierBarColor = barrierBarFill.color;
        

        TutorialMonsterIntent temp = enemyIntent as TutorialMonsterIntent;
        temp.defendAction.toDefend = this;
    }

    internal override void DecideIntent(BoardStateController board)
    {

        if(Defending)
        {

            defendFor = queuedDefence;
            queuedDefence = 0;
            Defending = false;
            animator.SetBool("isBlocking", true);

        }
        else
        {

            defendFor = 0;
            animator.SetBool("isBlocking", false);

        }

        damageSources.Clear();
        queuedDamage = 0;
        queuedPierce = 0;
        queuedPoison = 0;
        UpdateHealthDamageUI();

        enemyIntent.CancelIntent();

        UpdateDefenceUI();
        UpdateIntentUI();

    }

    public void TutorialIntentOverride(BoardStateController _board, TutorialActions _actionToPerform)
    {

        enemyIntent.TutorialIntentOverride(_board, _actionToPerform);
        UpdateDefenceUI();
        UpdateIntentUI();
    }

}
