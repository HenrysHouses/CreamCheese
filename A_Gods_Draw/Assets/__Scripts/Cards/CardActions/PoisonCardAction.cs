using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonCardAction : CardAction
{

    public PoisonCardAction(int strength) : base(strength, strength) { }

    public override IEnumerator OnAction(BoardStateController board, NonGod_Behaviour source)
    {
        isReady = false;

        foreach (IMonster target in targets)
        {
            
            PoisonDebuff _poison;
            if(target.gameObject.TryGetComponent<PoisonDebuff>(out _poison))
            {

                _poison.Stacks += strength;

            }
            else
            {

                _poison = target.gameObject.AddComponent<PoisonDebuff>();
                _poison.Stacks = strength;
                _poison.thisMonster = target;

            }

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
