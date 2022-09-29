using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TyrActions : IGodActions
{
    public override void OnPlay(God_Behaviour current, List<IMonster> enemies, PlayerController player, List<NonGod_Behaviour> lane)
    {
        base.OnPlay(current, enemies, player, lane);
    }
}
