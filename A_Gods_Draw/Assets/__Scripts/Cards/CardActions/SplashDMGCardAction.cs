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
        playSFX(source.gameObject);



        for (int i = 0; i < source.AllTargets.Length; i++)
        {
            for (int j = 0; j < board.Enemies.Length; j++)
            {
                Debug.Log("trigghgggggggerss");
                if(board.Enemies[i] != null)
                {
                    if(board.Enemies[i].GetHealth() > 0)
                        board.StartCoroutine(playTriggerVFX(
                            source.AllTargets[i].gameObject, 
                            board.Enemies[i].transform, 
                            new Vector3(0, 0 ,-0.2f)));
                }

                if(j-1 >= 0)
                {
                    if(board.Enemies[j-1] != null)
                    {
                        if(board.Enemies[j-1].GetHealth() > 0)
                            board.Enemies[j-1].TakeDamage((int)((source.stats.strength / 2f) + 0.6f));
                    }
                }

                if(j+1 < board.Enemies.Length)
                {
                    if(board.Enemies[j+1] != null)
                    {
                        if(board.Enemies[j+1].GetHealth() > 0)
                            board.Enemies[j+1].TakeDamage((int)((source.stats.strength / 2f) + 0.6f));
                    }
                }
            }
        }

        yield return new WaitUntil(() => !_VFX.isAnimating);
        isReady = true;
    }

    public override void SetActionVFX()
    {
        _VFX = new ActionVFX(false, 0, "", "Action VFX/Splash_VFX", 2);
    }
}