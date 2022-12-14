// Written by Javier Villegas
using UnityEngine;

public class TyrActions : GodCardAction
{
    public override void Execute(BoardStateController board, int strengh, UnityEngine.Object source) { }

    public override void OnPlay(BoardStateController board)
    {
        foreach (IMonster monster in board.getLivingEnemies())
        {
            monster.GetIntent().CancelIntent();
            monster.SetOverlay(Resources.Load<Sprite>("ImageResources/Icon_Chain_v1"));
        }
    }
}
