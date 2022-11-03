using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TyrActions : GodCardAction
{
    public override void Execute(BoardStateController board, int strengh)
    {
        
    }

    public override void OnPlay(BoardStateController board)
    {
        foreach (IMonster monster in board.Enemies)
        {
            monster.GetIntent().CancelIntent();
            monster.UpdateUI();
        }
    }
}
