using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrostbiteCardAction : CardAction
{

    public override IEnumerator OnAction(BoardStateController board, NonGod_Behaviour source)
    {
        isReady = false;

        foreach (IMonster target in targets)
        {
            
            FrostbiteDebuff _frostbite;
            if(target.gameObject.TryGetComponent<FrostbiteDebuff>(out _frostbite))
            {

                _frostbite.Stacks += cardStats.strength;

            }
            else
            {

                _frostbite = target.gameObject.AddComponent<FrostbiteDebuff>();
                _frostbite.Stacks = cardStats.strength;
                _frostbite.thisMonster = target;

            }

            target.DeBuff(cardStats.strength);

        }
        
        targets.Clear();

        // Playing VFX for each action
        board.StartCoroutine(playTriggerVFX(source.gameObject, board.Player.transform, new Vector3(0, 1, 0)));
        yield return new WaitUntil(() => _VFX == null || !_VFX.isAnimating);

        yield return new WaitForSeconds(0.3f);

        isReady = true;
    }

    public override void Reset(BoardStateController board)
    {
        targets.Clear();
        isReady = false;
        board.SetClickable(3, false);
        ResetCamera();
    }
    public override void ResetCamera()
    {
    }
    public override void SetCamera()
    {
    }

}
