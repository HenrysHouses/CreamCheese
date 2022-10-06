using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TyrActions : IGodAction
{
    public override void OnPlay(BoardState board)
    {
        foreach (IMonster monster in board.enemies)
        {
            monster.CancelIntents();
        }
    }
}
