// Written by Javier Villegas
using UnityEngine;

public class TyrActions : GodCardAction
{
    public override void Execute(BoardStateController board, int strength, UnityEngine.Object source) { }

    public override void OnPlay(BoardStateController board, int strength)
    {
        foreach (IMonster monster in board.getLivingEnemies())
        {
            monster.GetIntent().CancelIntent();
            monster.SetOverlay(Resources.Load<Sprite>("ImageResources/Icon_Chain_v1"));
        }
    }
}
