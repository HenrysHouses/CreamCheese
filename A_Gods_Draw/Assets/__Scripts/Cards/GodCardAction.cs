using UnityEngine;
using System.Collections.Generic;

// Written by Javier Villegas
// Edited by henrik
public abstract class GodCardAction : Action
{
    protected GameObject EnterTheBattleFieldVFX, EffectedTargetVFX;
    protected float ETB_VFX_Lifetime, Effected_VFX_Lifetime;
    protected MonoBehaviour[] EffectedTargetsForVFX;
    public GodCardAction()
    {
        SetActionVFX();
    }

    /// <summary>Should set the settings for the action's _VFX</summary>
    public abstract void SetActionVFX();
    public virtual void OnPlay(BoardStateController board, int strength) 
    {
        if(EnterTheBattleFieldVFX)
        {
            GameObject vfx = GameObject.Instantiate(EnterTheBattleFieldVFX);
            GameObject.Destroy(vfx, ETB_VFX_Lifetime);
        }

        Debug.Log(EffectedTargetVFX + ", " + EffectedTargetsForVFX.Length);

        if(EffectedTargetVFX)
        {
            for (int i = 0; i < EffectedTargetsForVFX.Length; i++)
            {

                GameObject VFX = GameObject.Instantiate(EffectedTargetVFX);
                VFX.transform.position = EffectedTargetsForVFX[i].transform.position;
                GameObject.Destroy(VFX, Effected_VFX_Lifetime);
            }
        }
    }
    public virtual void OnLeaveBoard(BoardStateController board) { }
    public virtual void OnTurnStart(BoardStateController board) { }
    public virtual void OnDrawPhase(BoardStateController board) { }
    public virtual void OnCombatStartPhase(BoardStateController board) { }
}
