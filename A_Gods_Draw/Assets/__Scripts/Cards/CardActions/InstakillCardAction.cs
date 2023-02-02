// Written by Javier Villegas
using System.Collections;
using UnityEngine;

public class InstakillCardAction : CardAction
{
    public override IEnumerator OnAction(BoardStateController board, NonGod_Behaviour source)
    {
        isReady = false;

        foreach (var enemy in cardStats.Targets)
        {
            if (Random.Range(1, 10) <= cardStats.strength)
            {
                // Playing VFX
                board.StartCoroutine(playTriggerVFX(enemy.gameObject, null, new Vector3(0, 1 ,0)));
                yield return new WaitUntil(() => !_VFX.isAnimating);
                enemy.GetComponent<IMonster>().DealDamage(10000);
            }
        }
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
