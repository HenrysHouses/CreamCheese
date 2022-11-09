using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashDMGCardAction : CardAction
{
    IMonster target;
    public SplashDMGCardAction(int strengh) : base(strengh, strengh) { }

    public override IEnumerator ChoosingTargets(BoardStateController board, float mult)
    {
        camAnim.SetBool("EnemyCloseUp", true);
        isReady = false;
        //foreach monster in bpard, enable click
        board.SetClickable(3);

        Debug.Log("waiting for selecting enemies...");

        yield return new WaitUntil(HasClickedMonster);

        camAnim.SetBool("EnemyCloseUp", false);

        board.SetClickable(3, false);

        isReady = true;
    }

    bool HasClickedMonster()
    {
        if (Input.GetMouseButtonDown(0))
        {
            BoardElement element = TurnController.PlayerClick();
            IMonster clickedMonster = element as IMonster;
            if (clickedMonster)
            {
                Debug.Log(clickedMonster);
                target = clickedMonster;
                return true;
            }
            current.MissClick();
        }
        return false;
    }

    public override IEnumerator OnAction(BoardStateController board)
    {
        isReady = false;
        //StartAnimations...

        //yield return new WaitUntil(() => true);
        yield return new WaitForSeconds(0.5f);

        if (target)
        {
            var enemies = Physics.SphereCastAll(target.transform.position, 0.3f, Vector3.one);
            foreach (RaycastHit allinside in enemies)
            {
                IMonster monster = allinside.collider.GetComponent<IMonster>();
                if (monster && monster != target)
                {
                    monster.DealDamage(strengh);
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
}