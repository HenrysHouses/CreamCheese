using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BoardTarget : MonoBehaviour , IMonsterTarget
{

    public BoardStateController Board;
    public bool IsActive = true;
    public int MaxHealth
    {
        get{return maxHealth;}
    }
    [SerializeField]
    protected int maxHealth;
    [SerializeField]
    protected int currentHealth;

    public abstract void DealDamage(int _amount, UnityEngine.Object _source = null);
    public virtual void Targeted(GameObject _sourceGO){}
    public virtual void UnTargeted(GameObject _sourceGO){}
    protected abstract void DeActivate();
    public abstract void ReActivate();

}
