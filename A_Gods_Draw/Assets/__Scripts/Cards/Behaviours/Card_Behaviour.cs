using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Card_Behaviour : MonoBehaviour
{
    public void Initialize(Card_SO card)
    {
        
    }
    public virtual void OnPlay() { }
    public virtual void OnAction() { }
}
