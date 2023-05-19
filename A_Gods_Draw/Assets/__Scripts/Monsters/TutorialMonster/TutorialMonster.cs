using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class TutorialMonster : Monster
{

    [SerializeField]
    private EventReference attackSFX, attackGodSFX, defendSFX;

    protected override void Start()
    {
        
        enemyIntent = new TutorialMonsterIntent(this, attackSFX, attackGodSFX, defendSFX);
        enemyIntent.Self = this;
        healthBarColor = healthBarFill.color;
        barrierBarColor = barrierBarFill.color;
        
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
