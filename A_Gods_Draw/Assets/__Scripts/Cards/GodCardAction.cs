// Written by Javier Villegas
public abstract class GodCardAction : Action
{
    public GodCardAction() : base(0, 0) { }

    public virtual void OnPlay(BoardStateController board) { }
    public virtual void OnLeaveBoard(BoardStateController board) { }
    public virtual void OnTurnStart(BoardStateController board) { }
    public virtual void OnDrawPhase(BoardStateController board) { }
    public virtual void OnCombatStartPhase(BoardStateController board) { }
}
