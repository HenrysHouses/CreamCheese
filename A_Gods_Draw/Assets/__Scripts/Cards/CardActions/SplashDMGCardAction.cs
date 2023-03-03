// Written by Javier Villegas
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SplashDMGCardAction : CardAction
{
    public override IEnumerator OnAction(BoardStateController board, ActionCard_Behaviour source)
    {
        isReady = false;
        //StartAnimations...

        for (int i = 0; i < source.AllTargets.Length; i++)
        {
            for (int j = 0; j < board.Enemies.Length; j++)
            {
                int SplashIndex;
                if(board.Enemies[j] == source.AllTargets[i]) 
                {
                    SplashIndex = j;
                }
                
                if(j-1 >= 0)
                    board.Enemies[j-1].TakeDamage((int)((source.stats.strength / 2f) + 0.6f));

                if(j+1 < board.Enemies.Length)
                    board.Enemies[j+1].TakeDamage((int)((source.stats.strength / 2f) + 0.6f));

                board.StartCoroutine(playTriggerVFX(
                    source.AllTargets[i].gameObject, 
                    board.Enemies[i].transform, 
                    new Vector3(0, 0 ,-0.2f)));
            }
        }

        yield return new WaitUntil(() => !_VFX.isAnimating);
        isReady = true;
    }
}