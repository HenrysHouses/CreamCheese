using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainCardAction : CardAction
{
    List<IMonster> targets = new();
    private int totalTargets = 1;

    public ChainCardAction(int strengh) : base(strengh, strengh) { }

    public override IEnumerator ChoosingTargets(BoardStateController board, float mult)
    {
        camAnim.SetBool("EnemyCloseUp", true);
        isReady = false;

        if (mult > 1.01f)
        {
            totalTargets = 2;
        }

        //foreach monster in bpard, enable click
        board.SetClickable(3);

        Debug.Log("waiting for selecting enemies...");

        yield return new WaitUntil(HasClickedMonsters);

        camAnim.SetBool("EnemyCloseUp", false);

        board.SetClickable(3, false);

        isReady = true;
    }

    bool HasClickedMonsters()
    {

        BoardElement element = TurnController.PlayerClick();
        IMonster clickedMonster = element as IMonster;
        if (clickedMonster)
        {
            Debug.Log(clickedMonster);

            if (totalTargets == 1)
            {
                targets.Add(clickedMonster);
                return true;
            }
            else if (totalTargets == 2)
            {
                if (targets.Count == 0)
                {
                    targets.Add(clickedMonster);
                    return false;
                }
                if (clickedMonster != targets[0])
                {
                    targets.Add(clickedMonster);
                    if (targets.Count == totalTargets)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public override IEnumerator OnAction(BoardStateController board, int strengh)
    {
        isReady = false;
        //StartAnimations...

        //yield return new WaitUntil(() => true);
        yield return new WaitForSeconds(0.5f);

        foreach (IMonster monster in targets)
        {
            if (monster)
            {
                monster.GetIntent().CancelIntent();
                monster.UpdateUI();
            }
        }

        targets.Clear();

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
        camAnim.SetBool("EnemyCloseUp", false);
    }
}
