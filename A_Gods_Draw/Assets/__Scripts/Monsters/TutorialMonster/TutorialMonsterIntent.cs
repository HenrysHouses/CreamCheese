using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class TutorialMonsterIntent : Intent
{

    private AttackPlayerAction attackPlayerAction;
    private AttackGodAction attackGodAction;
    public DefendAction defendAction;
    public TutorialMonsterIntent(EventReference _attackSFX, EventReference _attackGodSFX, EventReference _defendSFX)
    {

        attackPlayerAction = new AttackPlayerAction(4, 4);
        attackPlayerAction.ActionSFX = _attackSFX;
        attackGodAction = new AttackGodAction(2, 2);
        attackGodAction.ActionSFX = _attackGodSFX;
        defendAction = new DefendAction(1, 1);
        defendAction.ActionSFX = _defendSFX;

    }

    public override void DecideIntent(BoardStateController _board){}

    public override void TutorialIntentOverride(BoardStateController _board, TutorialActions _actionToPerform)
    {
        switch (_actionToPerform)
        {
            
            case TutorialActions.AttackPlayer:
            actionSelected = attackPlayerAction;
            break;

            case TutorialActions.AttackGod:
            actionSelected = attackGodAction;
            break;

            case TutorialActions.Defend:
            actionSelected = defendAction;
            break;

        }

        if (actionSelected != null)
            strength = Random.Range(actionSelected.MinStrength, actionSelected.MaxStrength + 1);

    }

    public override void LateDecideIntent(BoardStateController _board){}
    public override void CancelIntent(){}

}

public enum TutorialActions
{

    AttackPlayer,
    AttackGod,
    Defend

}