// Written by Javier Villegas
using UnityEngine;

[System.Serializable]
public class TyrActions : GodCardAction
{
    public override void Execute(BoardStateController board, int strength, UnityEngine.Object source) { }

    public override void OnPlay(BoardStateController board, int strength)
    {
        foreach (Monster monster in board.getLivingEnemies())
        {
            DebuffBase _debuff = monster.gameObject.AddComponent<ChainedDebuff>();
            _debuff.Stacks = 1;
            _debuff.thisMonster = monster;
        }
    }
}
