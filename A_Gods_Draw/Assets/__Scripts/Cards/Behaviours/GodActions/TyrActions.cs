using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TyrActions : IGodAction
{
    public override void OnPlay(BoardStateController board)
    {
        foreach (IMonster monster in board.Enemies)
        {
            monster.GetIntent().CancelIntent();
        }
    }
}
