// Written by Javier Villegas
using System.Collections;
using UnityEngine;

public class DefendCardAction : CardAction
{
    //bool HasClickedMonster()
    //{
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        BoardElement element = TurnController.PlayerClick();
    //        IMonster clickedMonster = element as IMonster;
    //        if (clickedMonster)
    //        {
    //            Debug.Log(clickedMonster);
    //            target = clickedMonster;
    //            return true;
    //        }
    //        current.MissClick();
    //    }
    //    return false;
    //}

    public override IEnumerator OnAction(BoardStateController board, NonGod_Behaviour source)
    {
        isReady = false;

        foreach (IMonster target in cardStats.Targets)
        {
            if (target)
            {
                // Playing VFX for each action
                board.StartCoroutine(playTriggerVFX(source.gameObject, target));
                yield return new WaitUntil(() => !_VFX.isAnimating);
                target.DeBuff(cardStats.strength);
            }
        }
        cardStats.Targets.Clear();

        yield return new WaitForSeconds(0.4f);

        isReady = true;
    }
    
    public override void ResetCamera()
    {
        camAnim.SetBool("EnemyCloseUp", false);
    }
    public override void SetCamera()
    {
        camAnim.SetBool("EnemyCloseUp", true);
    }
}
