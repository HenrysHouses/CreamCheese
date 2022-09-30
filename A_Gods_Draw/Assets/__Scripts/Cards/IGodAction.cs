using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IGodAction : MonoBehaviour
{
    public virtual void OnPlay(BoardState board) { }
}
