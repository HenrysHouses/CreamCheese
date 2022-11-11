using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashDMGCardAction : CardAction
{
    IMonster target;
    public SplashDMGCardAction(int strengh) : base(strengh, strengh) { }


    public override IEnumerator OnAction(BoardStateController board)
    {
        isReady = false;
        //StartAnimations...

        //yield return new WaitUntil(() => true);
        yield return new WaitForSeconds(0.5f);

        if (target)
        {
            var enemies = Physics.SphereCastAll(target.transform.position, 0.6f, Vector3.one);
            foreach (RaycastHit allinside in enemies)
            {
                IMonster monster = allinside.collider.GetComponent<IMonster>();
                if (monster && monster != target)
                {
                    monster.DealDamage((int)((strengh / 2f) + 0.6f));
                }
            }
        }

        target = null;

        isReady = true;
    }

    public override void Reset(BoardStateController board)
    {
        target = null;
        isReady = false;
        board.SetClickable(3, false);
        ResetCamera();
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