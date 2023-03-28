// Written by Javier Villegas
using System.Collections;
using UnityEngine;

[System.Serializable]
public class InstakillCardAction : CardAction
{
    public override IEnumerator OnAction(BoardStateController board, ActionCard_Behaviour source)
    {
        isReady = false;

        foreach (var enemy in source.AllTargets)
        {
            if (Random.Range(1, 20) <= source.stats.strength)
            {
                // Playing VFX
                playSFX(source.gameObject);
                board.StartCoroutine(playTriggerVFX(enemy.gameObject, null, new Vector3(0, 1 ,0)));
                yield return new WaitUntil(() => !_VFX.isAnimating);
                enemy.GetComponent<Monster>().TakeDamage(10000);
            }
        }
        isReady = true;
    }

    public override void SetActionVFX()
    {
        _VFX = new ActionVFX(false, 0, "", "", 0);
        Debug.LogError("Instant Kill has no VFX");
    }
}
