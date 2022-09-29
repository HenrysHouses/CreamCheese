using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IGodActions : MonoBehaviour
{
    public virtual void OnPlay(God_Behaviour current, List<IMonster> enemies, PlayerController player, List<NonGod_Behaviour> lane) { }
}
