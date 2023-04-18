// Written by Javier Villegas
using System.Collections;
using UnityEngine;

[System.Serializable]
public class HealCardAction : CardAction
{
    // public override void SetClickableTargets(BoardStateController board, bool to = true)
    // {
    // }

    public override IEnumerator OnAction(BoardStateController board, ActionCard_Behaviour source)
    {
        isReady = false;
        playSFX(source.gameObject);
        board.Player.Heal(source.stats.strength);
        // Playing VFX for each action
        board.StartCoroutine(playTriggerVFX(source.gameObject, board.Player.transform, new Vector3(0, 1, 0)));
        yield return new WaitUntil(() => _VFX == null || !_VFX.isAnimating);

        yield return new WaitForSeconds(0.3f);

        isReady = true;
    }

    public override void Reset(BoardStateController board, Card_Behaviour Source)
    {
        ActionCard_Behaviour card = Source as ActionCard_Behaviour;
        // card.stats.Targets.Clear();
        isReady = false;
    }

    public override void SetActionVFX()
    {
        _VFX = new ActionVFX(false, 0, "", "Action VFX/Heal_VFX", 5);
        
        Debug.LogError("Heal has no VFX");
    }
}
