using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BoardTarget : MonoBehaviour
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

    public abstract void TakeDamage(int _amount);
    protected abstract void DeActivate();
    public abstract void ReActivate();

}
