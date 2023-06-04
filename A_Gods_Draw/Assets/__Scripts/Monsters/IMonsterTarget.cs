using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMonsterTarget
{

    public void DealDamage(int _amount, UnityEngine.Object _source = null);
    public void Targeted(GameObject _sourceGO = null);
    public void UnTargeted(GameObject _sourceGO = null);
    public Transform GetTransform();

}
