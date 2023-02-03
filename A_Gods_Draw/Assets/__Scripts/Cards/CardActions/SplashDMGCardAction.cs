// Written by Javier Villegas
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashDMGCardAction : CardAction
{
    List<Vector3> splashCenter = new();

    public override IEnumerator OnAction(BoardStateController board, NonGod_Behaviour source)
    {
        isReady = false;
        //StartAnimations...

        //yield return new WaitUntil(() => true);
        yield return new WaitForSeconds(0.2f);

        for (int i = 0; i < splashCenter.Count; i++)
        {
            var enemies = Physics.SphereCastAll(splashCenter[i], 0.3f, Vector3.one);
            foreach (RaycastHit allinside in enemies)
            {
                IMonster monster = allinside.collider.GetComponent<IMonster>();
                if (monster && monster != source.stats.Targets[i])
                {
                    // Playing VFX
                    board.StartCoroutine(playTriggerVFX(source.stats.Targets[i].gameObject, null, new Vector3(0, 1 ,0)));
                    monster.DealDamage((int)((source.stats.strength / 2f) + 0.6f));
                }
            }
        }

        yield return new WaitUntil(() => !_VFX.isAnimating);

        source.stats.Targets.Clear();
        splashCenter.Clear();

        isReady = true;
    }

   public override void Reset(BoardStateController board, Card_Behaviour Source)
    {
        NonGod_Behaviour card = Source as NonGod_Behaviour;
        card.stats.Targets.Clear();
        splashCenter.Clear();
        isReady = false;
        board.SetClickable(3, false);
    }

    // internal override void AddTarget(BoardElement target)
    // {
    //     base.AddTarget(target);
    //     splashCenter.Add(target.transform.position);
    // }
}