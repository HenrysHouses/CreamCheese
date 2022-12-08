// Written by Javier Villegas
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashDMGCardAction : CardAction
{
    List<Vector3> splashCenter = new();
    public SplashDMGCardAction(int strengh) : base(strengh, strengh) { }

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
                if (monster && monster != targets[i])
                {
                    // Playing VFX
                    board.StartCoroutine(playTriggerVFX(targets[i].gameObject, null, new Vector3(0, 1 ,0)));
                    monster.DealDamage((int)((strengh / 2f) + 0.6f));
                }
            }
        }

        yield return new WaitUntil(() => !_VFX.isAnimating);

        targets.Clear();
        splashCenter.Clear();

        isReady = true;
    }

    public override void Reset(BoardStateController board)
    {
        targets.Clear();
        splashCenter.Clear();
        isReady = false;
        board.SetClickable(3, false);
        ResetCamera();
    }

    internal override void AddTarget(BoardElement target)
    {
        base.AddTarget(target);
        splashCenter.Add(target.transform.position);
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