using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardTarget : MonoBehaviour
{

    public BoardStateController Board;
    public int MaxHealth
    {
        get{return maxHealth;}
    }
    [SerializeField]
    protected int maxHealth;
    [SerializeField]
    protected int currentHealth;

    public virtual void TakeDamage(int _amount){}

}
