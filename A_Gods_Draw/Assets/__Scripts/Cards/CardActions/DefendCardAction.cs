// Written by Javier Villegas
using System.Collections;
using UnityEngine;

[System.Serializable]
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

    public override IEnumerator OnAction(BoardStateController board, ActionCard_Behaviour source)
    {
        isReady = false;

        foreach (BoardElement target in source.AllTargets)
        {
            Monster Enemy = target as Monster;

            if (Enemy)
            {
                // Playing VFX for each action
                board.StartCoroutine(playTriggerVFX(source.gameObject, Enemy));

                if(_VFX != null)
                    yield return new WaitUntil(() => !_VFX.isAnimating);
                Enemy.DeBuff(source.stats.strength);
            }
        }
        // source.stats.Targets.Clear();

        yield return new WaitForSeconds(0.4f);

        isReady = true;
    }
}
